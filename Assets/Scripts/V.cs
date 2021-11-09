using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class V : MonoBehaviourPun
{
    [SerializeField]
    M Model;
    // Start is called before the first frame update
    void Start()
    {
        Model = GetComponent<M>();

        Model.OnGhostEnd += GhostFormEndRPC;
        Model.OnGhostStart += GhostFormStartRPC;
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
