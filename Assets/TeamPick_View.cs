using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TeamPick_View : MonoBehaviourPun
{
    [SerializeField]
    TextMeshProUGUI TeamA;

    [SerializeField]
    TextMeshProUGUI TeamB;

    [SerializeField]
    TextMeshProUGUI WaitTeam;

    [SerializeField]
    LogInTeamManager teamManager;
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            teamManager.OnTeamWaitUpdate += TeamWaitHandler;
            teamManager.OnTeamAUpdate += TeamAHandler;
            teamManager.OnTeamBUpdate += TeamBHandler;
            teamManager.OnPlayerJoin += ViewHandler;
        }

        TeamA.richText = true;
        TeamB.richText = true;
        WaitTeam.richText = true;
    }
    
    string PlayerListToString(List<Player> list)
    {
        string newString="";

        foreach (var player in list)
        {
            newString = newString + "/" + player.NickName;
        }

        return newString;
    }
   
    void TeamWaitHandler()
    {
        photonView.RPC("TeamWaitUpdate",RpcTarget.All,PlayerListToString(teamManager.WaitTeam));
    }
    void TeamAHandler()
    {
        photonView.RPC("TeamAUpdate", RpcTarget.All, PlayerListToString(teamManager.TeamA));
    }
    void TeamBHandler()
    {
        photonView.RPC("TeamBUpdate", RpcTarget.All, PlayerListToString(teamManager.TeamB));

    }
    void ViewHandler(Player newPlayer)
    {
        photonView.RPC("TeamWaitUpdate", newPlayer, PlayerListToString(teamManager.WaitTeam));
        photonView.RPC("TeamAUpdate", newPlayer, PlayerListToString(teamManager.TeamA));
        photonView.RPC("TeamBUpdate", newPlayer, PlayerListToString(teamManager.TeamB));

    }

    [PunRPC]
    void TeamWaitUpdate(string newText)
    {
        var nickNames = newText.Split('/');

        string text = "";

        foreach (var playerNickName in nickNames)
        {

            if (playerNickName == PhotonNetwork.LocalPlayer.NickName)
            {

                text = text + "<color=green>" + playerNickName + "  </color>" + "\n";
            }
            else
            {
                text = text + "<color=black>" + playerNickName + "  </color>" + "\n";
            }
        }

        WaitTeam.text = text;
    }
    [PunRPC]
    void TeamAUpdate(string newText)
    {
        var nickNames = newText.Split('/');

        string text = "";

        foreach (var playerNickName in nickNames)
        {

            if (playerNickName == PhotonNetwork.LocalPlayer.NickName)
            {

                text = text + "<color=green>" + playerNickName + "  </color>" + "\n";
            }
            else
            {
                text = text + "<color=black>" + playerNickName + "  </color>" + "\n";
            }
        }

        TeamA.text = text;
    }
    [PunRPC]
    void TeamBUpdate(string newText)
    {
        var nickNames = newText.Split('/');

        string text = "";

        foreach (var playerNickName in nickNames)
        {

            if (playerNickName == PhotonNetwork.LocalPlayer.NickName)
            {

                text = text + "<color=green>" + playerNickName + "  </color>" + "\n";
            }
            else
            {
                text = text + "<color=black>" + playerNickName + "  </color>" + "\n";
            }
        }

        TeamB.text = text;
    }


}
