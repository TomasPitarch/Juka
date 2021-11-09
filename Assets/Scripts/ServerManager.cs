using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System;

public class ServerManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    ClientManager clientManager;

    //Dictionary<Player, string> PlayerList;



    void Start()
    {
        //PlayerList = new Dictionary<Player, string>();

        clientManager = GameObject.Find("ClientManager").GetComponent<ClientManager>();

        photonView.RPC("CreateCharacter_Request",RpcTarget.MasterClient,PhotonNetwork.LocalPlayer);

    }

    [PunRPC]
    public void CreateCharacter_Request(Player newPlayer)
    {
        clientManager.photonView.RPC("CreatePlayer", newPlayer);
    }
    
    
    
}
