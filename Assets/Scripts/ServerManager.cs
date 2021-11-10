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
    ClientManager clientManager;

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
}
