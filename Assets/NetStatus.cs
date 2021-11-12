using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using System;
using UnityEngine;

public class NetStatus : MonoBehaviourPun
{
    [SerializeField]
    float statusTime;

    public event Action OnNetReleased = delegate { };

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Trapped(statusTime);
    }
    public void Init(Vector3 position)
    {
        transform.position = position;
    }
    public void Destroy()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        PhotonNetwork.Destroy(gameObject);
    }
    async void Trapped(float time)
    {
        await Task.Delay((int)time * 1000);
        OnNetReleased();
        Destroy();

    }
}
