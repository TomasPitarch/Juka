using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class QuickLoggin : MonoBehaviourPunCallbacks
{

    [SerializeField]
    InputField textName;

    [SerializeField]
    Button connectButton;

    bool _joinedLobby = false;  

    void Start()
    {
        connectButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        var nicknameText = textName.text;
       
        if ((!string.IsNullOrEmpty(nicknameText) && !string.IsNullOrWhiteSpace(nicknameText)) && _joinedLobby)
        {
            connectButton.interactable = true;
        }
        else
        {
            connectButton.interactable = false;
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        _joinedLobby = true;  
    }
    public override void OnCreatedRoom()
    {
        //print("me cree sala");
    }
    public void ConnectToRoom()
    {
        //Create Room Config//
        var roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 10;

        //Create Room with the previous config//
        PhotonNetwork.JoinOrCreateRoom("Sala de test", roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        // Set the Player Nickname//
        PhotonNetwork.LocalPlayer.NickName = textName.text;
        PhotonNetwork.LoadLevel("TeamsScene");

    }


}
