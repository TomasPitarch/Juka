using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

public class HookSkill : SkillShoot
{

    [SerializeField]
    HookSkill_SO data;

    Hook hookHead;

    [SerializeField]
    GameObject skillSpawnPoint;

    

    
    private void Start()
    {
        _cooldown = false;
        _cdTime = data._coolDownTime;
       
    }
    public bool CanSkill()
    {
        return !_cooldown;
    }
    public override void CastSkillShoot(Vector3 point)
    {
        if (CanSkill())
        {

            print("2-El skill skillea");
            SkillShoot_SVRequest(point);
            CoolDownTimer();
        }
       
    }
    void SkillShoot_SVRequest(Vector3 point)
    {

        print("3-Hago un request al server");
        photonView.RPC("SkillingHook", RpcTarget.MasterClient,point);
    }

    [PunRPC]
    void SkillingHook(Vector3 point)
    {

        print("4-Server skillea el hook");
        var hook = SpawnHook();
           
        var SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
        hook.Init(skillSpawnPoint, data, SkillDirection, photonView.ViewID);
        
    }
    
    Hook SpawnHook()
    {
        print("5-Spawnea el hook");
        //HookHeadCreation and event suscription//
        var newHook = PhotonNetwork.Instantiate("Hook", Vector3.zero, Quaternion.identity).GetComponent<Hook>();
        return newHook;
    }

   
}
