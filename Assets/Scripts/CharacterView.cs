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
        myModel.OnStop += StopAnimation;
        myModel.OnHookShoot += HookAnimation;
        myModel.OnNetShoot += NetAnimation;

    }

    void MoveAnimation(Vector3 nada)
    {
        //Debug.Log("animamos a la derecha");
        myAnim.SetBool("Idle", false);            
        myAnim.SetTrigger("Moving");
    }
       
    void IdleAnimation()
    {
        //Debug.Log("Idle");

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

    void StopAnimation()
    {
        myAnim.SetBool("Moving", false);
        myAnim.SetBool("Idle", false);
        myAnim.SetTrigger("Stop");
    }
}
