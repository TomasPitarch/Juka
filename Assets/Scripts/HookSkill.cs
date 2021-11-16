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
    
    public override void CastSkillShoot(Vector3 point)
    {
        if (_cooldown)
        {
            return;
        }
        SkillShoot_SVRequest(point);
        CoolDownTimer();
    }
    void SkillShoot_SVRequest(Vector3 point)
    {
        photonView.RPC("SkillingHook", RpcTarget.MasterClient,point);
    }

    [PunRPC]
    void SkillingHook(Vector3 point)
    {
     
            var hook = SpawnHook();
           
            var SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
            hook.Init(skillSpawnPoint, data, SkillDirection, photonView.ViewID);
        
    }
    
    Hook SpawnHook()
    {
        //HookHeadCreation and event suscription//
        var newHook = PhotonNetwork.Instantiate("Hook", Vector3.zero, Quaternion.identity).GetComponent<Hook>();
        return newHook;
    }

   
}
