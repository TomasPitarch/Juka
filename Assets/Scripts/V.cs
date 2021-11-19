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
        //canvas.transform.LookAt(new Vector3(transform.position.x,
        //                                    transform.position.y,
        //                                    Camera.main.transform.position.z*-1));

        Vector3 v = Camera.main.transform.position - transform.position;
        v.x = v.z = 0.0f;
        canvas.transform.LookAt(Camera.main.transform.position - v);
        canvas.transform.Rotate(0, 180, 0);
    }

   
}
