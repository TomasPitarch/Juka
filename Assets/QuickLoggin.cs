using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class QuickLoggin : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("me conecte");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("me joinie lobby");
        var roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 10;


        PhotonNetwork.JoinOrCreateRoom("Sala de test", roomOptions, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        print("me cree sala");
    }

    public override void OnJoinedRoom()
    {
        print("me joinie sala");
    }
}
