using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HookHead : MonoBehaviour
{
    public event Action OnObjectCollision = delegate { };
    public event Action OnHooksEnd = delegate { };

    M CharacterHooked;
    public Team team;
    private void Start()
    {
        HookHeadOff();
    }
    public void Init(Team teamToSet)
    {
        team = teamToSet;
    }
    public void HookHeadActive(Vector3 FromActivePosition)
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        transform.position = FromActivePosition;
    }

    public void HookHeadOff()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        CharacterHooked = null;
        OnHooksEnd();
    }
    
    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag=="Character")
        {
            print("colision de hook con character");

            CharacterHooked = collision.gameObject.GetComponent<M>();
            CharacterHooked.Hooked(this);
           
            
            OnObjectCollision();
        }
    }

    public void Move(Vector3 distanceToMove)
    {
        transform.position += distanceToMove;

        if(CharacterHooked)
        {
            CharacterHooked.transform.position += distanceToMove;
        }
    }

    
}
