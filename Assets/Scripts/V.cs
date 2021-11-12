using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class V : MonoBehaviourPun
{
    [SerializeField]
    M Model;

    [SerializeField]
    TextMeshProUGUI textNickName;

    [SerializeField]
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        Model = GetComponent<M>();

        Model.OnGhostEnd += GhostFormEndRPC;
        Model.OnGhostStart += GhostFormStartRPC;

        Model.OnDie += GhostFormStartRPC;
        Model.OnRespawn += GhostFormEndRPC;


        if(photonView.IsMine)
        {
            photonView.RPC("SetNickNameOnNet",RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
        }
        
    }
    [PunRPC]
    void SetNickNameOnNet(string nickName)
    {
        textNickName.text = nickName;
    }

    private void Update()
    {
        canvas.transform.LookAt(new Vector3(transform.position.x,
                                            transform.position.y,
                                            Camera.main.transform.position.z*-1));
    }


    [PunRPC]
    void GhostFormStart()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    [PunRPC]
    void GhostFormEnd()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    void GhostFormStartRPC()
    {
        photonView.RPC("GhostFormStart",RpcTarget.All);
    }
    void GhostFormEndRPC()
    {
        photonView.RPC("GhostFormEnd", RpcTarget.All);
    }
}
