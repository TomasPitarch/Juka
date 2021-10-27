using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HookHead : MonoBehaviour
{
    public event Action OnObjectCollision = delegate { };

    private void Start()
    {
        HookHeadOff();
    }
    public void HookHeadActive(Vector3 FromActivePosition)
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        transform.position = FromActivePosition;
    }

    public void HookHeadOff()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    
    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag=="Character")
        {
            print("colision de hook con character");
            collision.gameObject.GetComponent<M>().Hooked();
            OnObjectCollision();
        }
    }

    public void Move(Vector3 distanceToMove)
    {
        transform.position += distanceToMove;
    }
}
