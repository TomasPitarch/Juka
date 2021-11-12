using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System;

public sealed class ServerManager:MonoBehaviourPun
{
    private static ServerManager _instance ;
    public static ServerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    int maxScore=5;

    int TeamAScore=0;
    int TeamBScore = 0;

    [SerializeField]
    ClientManager clientManager;

    public event Action<int,int> OnScoreChange;

    Dictionary<int , Player> PlayerList;
    public List<int> IdList;

    void Start()
    {
        //Singleton//
        if(_instance!=null)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        //-----------------------//
        IdList = new List<int>();

        PlayerList = new Dictionary<int,Player>();
        clientManager = GetComponent<ClientManager>();

        photonView.RPC("CreateCharacter_Request",RpcTarget.MasterClient,PhotonNetwork.LocalPlayer);

    }

    [PunRPC]
    void CreateCharacter_Request(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        clientManager.photonView.RPC("CreatePlayer", newPlayer);
    }
    
    [PunRPC]
    void RegisterCharacter(int ID,Player player)
    {
        PlayerList.Add(ID, player);
        IdList.Add(ID);
    }

    public Player GetPlayer(int charPV_ID)
    {
        if (PlayerList.ContainsKey(charPV_ID))
        {
            return PlayerList[charPV_ID];
        }
        else
        {
            print("no existe player");
            return null;
        }


        
    }

    [PunRPC]
    public void CharacterDie_Request(int CharacterID)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        CharacterDie(CharacterID);
    }
    void CharacterDie(int CharacterID)
    {
        var character = PhotonView.Find(CharacterID);
        if (character.GetComponent<M>().myTeam == Team.A)
        {
            TeamBScore++;
        }
        else
        {
            TeamAScore++;
        }

        OnScoreChange(TeamAScore, TeamBScore);
        WinConditionCheck();
    }

    void WinConditionCheck()
    {
        if(TeamAScore>=maxScore)
        {
            
            photonView.RPC("Win",RpcTarget.All,Team.A);
        }
        else if(TeamBScore >= maxScore)
        {
            photonView.RPC("Win", RpcTarget.All, Team.B);
        }
    }

    [PunRPC]
    void Win(Team winnerTeam)
    {
        if(clientManager.MyTeam==winnerTeam)
        {
            print("Win");
        }
        else
        {
            print("Lose");
        }
    }
    

}
