using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

public class HookSkill : Skill,IReseteable
{

    [SerializeField]
    HookSkill_SO data;

    HookHead hookHead;

    [SerializeField]
    GameObject skillSpawnPoint;

    Team myTeam;

    List<HookHead> ListOfHooks;
    private void Start()
    {
        _cooldown = false;
        ListOfHooks = new List<HookHead>();

    }
    
    public void CastHook(Vector3 point)
    {
        if (!_cooldown && ListOfHooks.Count<1)
        {

            print("me da el CD del hook, lo casteo");
            var hook = SpawnHook();
            hook.OnHooksDestroy += HookHandler;
            var SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
            hook.Init(skillSpawnPoint, data, SkillDirection);

            ListOfHooks.Add(hook);

            CoolDownTimer();
        }
    }
    HookHead SpawnHook()
    {
        //HookHeadCreation and event suscription//
        var newHook = PhotonNetwork.Instantiate("Hook", Vector3.zero, Quaternion.identity).GetComponent<HookHead>();
        //OnHooksEnd+=;

        print("Spawneo un hook");
        return newHook;
    }
   
   void HookHandler(HookHead hook)
    {
        ListOfHooks.Remove(hook);
    }
    public void ResetCDs()
    {
        _tokenCoolDownTimer = false;
        _cooldown = false;
    }
}
