using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    Animator myAnim;
    M myModel;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myModel = GetComponent<M>();

        //Suscripcion a los eventos del Char//
        myModel.OnMove += MoveAnimation;
        myModel.OnIdle += IdleAnimation;
        myModel.OnTrapped += NetCaughtAnimation;
        myModel.OnHooked += FriendlyHookedAnimation;
        myModel.OnHookShoot += HookAnimation;
        myModel.OnNetShoot += NetAnimation;
        myModel.OnGhostStart += ShiftAnimation;
        myModel.OnDie += DieAnimation;
        myModel.OnRespawn += RespawnAnimation;
       

    }

    void MoveAnimation(Vector3 nada)
    {
                        
        myAnim.SetTrigger("Moving");
        //print("Move animation");
    }
       
    void IdleAnimation()
    {
        
        myAnim.SetTrigger("Idle");
    }

    void HookAnimation()
    {
        
        myAnim.SetTrigger("HookShoot");

    }
    void NetAnimation()
    {
        
        myAnim.SetTrigger("NetShoot");
    }

    void NetCaughtAnimation()
    {
        
        myAnim.SetTrigger("NetCaught");
    }

    void ShiftAnimation()
    {
       // print("SHIFT ANIM");
        
        myAnim.SetTrigger("Shift");
    }

    void FriendlyHookedAnimation()
    {
        print("HOOKED ANIM");
     
        myAnim.SetTrigger("FriendlyHooked");
    }
    void DieAnimation()
    {
        print("DIE ANIM");
        
        myAnim.SetTrigger("Die");
    }
    void RespawnAnimation()
    {
        print("Respawn ANIM");
       
        myAnim.SetTrigger("Respawn");
    }
}
