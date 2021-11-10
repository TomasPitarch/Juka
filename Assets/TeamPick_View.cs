using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
        }

        TeamA.richText = true;
        TeamB.richText = true;
        WaitTeam.richText = true;
    }

   
    void TeamWaitHandler()
    {
            string text="";

            foreach (var player in teamManager.WaitTeam)
            {

                if (player == PhotonNetwork.LocalPlayer)
                {
                    print("iguales");
                    text = text + "<color=green>" + player.NickName + "  </color>" + "\n";
                }
                else
                {
                    print("diferentes");
                    text = text + "<color=black>" + player.NickName + "  </color>" + "\n";
                }
            }

            photonView.RPC("TeamWaitUpdate", RpcTarget.All, text);

    }
    void TeamAHandler()
    {
        string text = "";
        foreach (var player in teamManager.TeamA)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                text = text + "<color=green>" + player.NickName + "  </color>" + "\n";
            }
            else
            {
                text = text + "<color=black>" + player.NickName + "  </color>" + "\n";
            }

        }

        photonView.RPC("TeamAUpdate", RpcTarget.All, text);

    }
    void TeamBHandler()
    {
        string text = "";
        foreach (var player in teamManager.TeamB)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                text = text + "<color=green>" + player.NickName + "  </color>" + "\n";
            }
            else
            {
                text = text + "<color=black>" + player.NickName + "  </color>" + "\n";
            }
        }

        photonView.RPC("TeamBUpdate", RpcTarget.All, text);

    }

    [PunRPC]
    void TeamWaitUpdate(string newList)
    {
        WaitTeam.text = newList;
    }
    [PunRPC]
    void TeamAUpdate(string newList)
    {
        TeamA.text = newList;
    }
    [PunRPC]
    void TeamBUpdate(string newList)
    {
        TeamB.text = newList;
    }


}
