using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

public class NetStatus : MonoBehaviour
{
    [SerializeField]
    float statusTime;

    public event Action OnNetReleased = delegate { };
    
    public void Init(Vector3 position)
    {
        transform.position = position;
        Trapped(statusTime);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    async void Trapped(float time)
    {
        await Task.Delay((int)time * 1000);
        OnNetReleased();
        Destroy();

    }
}
