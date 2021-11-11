using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ScoreView : MonoBehaviourPun
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        ServerManager.Instance.OnScoreChange += UpdateScoreHandler;
    }

    void UpdateScoreHandler(int TeamA, int TeamB)
    {
        photonView.RPC("UpdateView",RpcTarget.All, TeamA, TeamB);
    }


    [PunRPC]
    void UpdateView(int TeamA,int TeamB)
    {
        scoreText.text= "Team A:"+TeamA+" || "+TeamB+":Team B";
    }
        
}
