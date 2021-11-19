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
        
        myAnim.SetBool("Idle", false);            
        myAnim.SetBool("Moving", true);
        //print("Move animation");
    }
       
    void IdleAnimation()
    {
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", true);
    }

    void HookAnimation()
    {
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("HookShoot");

    }
    void NetAnimation()
    {
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("NetShoot");
    }

    void NetCaughtAnimation()
    {
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("NetCaught");
    }

    void ShiftAnimation()
    {
       // print("SHIFT ANIM");
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("Shift");
    }

    void FriendlyHookedAnimation()
    {
        print("HOOKED ANIM");
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("FriendlyHooked");
    }
    void DieAnimation()
    {
        print("DIE ANIM");
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("Die");
    }
    void RespawnAnimation()
    {
        print("Respawn ANIM");
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("Respawn");
    }
}
