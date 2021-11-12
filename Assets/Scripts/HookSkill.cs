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

    

    List<Hook> ListOfHooks;
    private void Start()
    {
        _cooldown = false;
        ListOfHooks = new List<Hook>();
    }
    
    public override void CastSkillShoot(Vector3 point)
    {
        if (_cooldown)
        {
            return;
        }
        SkillShoot_SVRequest(point);
    }
    void SkillShoot_SVRequest(Vector3 point)
    {
        photonView.RPC("SkillingHook", RpcTarget.MasterClient,point);
    }

    [PunRPC]
    void SkillingHook(Vector3 point)
    {
        if (ListOfHooks.Count < 1)
        {
            var hook = SpawnHook();
            hook.OnHooksDestroy += HookHandler;

            var SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
            hook.Init(skillSpawnPoint, data, SkillDirection, photonView.ViewID);

            ListOfHooks.Add(hook);

            CoolDownTimer();
        }
    }
    
    Hook SpawnHook()
    {
        //HookHeadCreation and event suscription//
        var newHook = PhotonNetwork.Instantiate("Hook", Vector3.zero, Quaternion.identity).GetComponent<Hook>();
        return newHook;
    }
   
   void HookHandler(Hook hook)
    {
        ListOfHooks.Remove(hook);
    }
   
}
