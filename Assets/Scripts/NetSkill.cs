using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetSkill : MonoBehaviour
{

    [SerializeField]
    NetSkill_SO data;

    Net net;

    [SerializeField]
    GameObject skillSpawnPoint;

    bool _cooldown;
    float _cdTime;
    float _skillRange;
    float _skillSpeed;

    bool _netColition;
    Vector3 SkillDirection;
    private void Start()
    {

        //Datatake//
        _cdTime = data._coolDownTime;
        _skillRange = data._skillRange;
        _skillSpeed = data._skillSpeed;
        _cooldown = false;
        _netColition = false;

        //HookHeadCreation and event suscription//
        net = Instantiate(data.hookHead).GetComponent<HookHead>();
        net.OnObjectCollision += HookColitionHanlder;

    }
   
    void HookColitionHanlder()
    {
        _netColition = true;
    }
    public void CastNet(Vector3 point)
    {
        point.y = skillSpawnPoint.transform.position.y;

        if (!_cooldown)
        {
            SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
            HookBehaviour();
            CoolDownTimer();
        }
    }
    async void CoolDownTimer()
    {
        print("TimerComienza");
        _cooldown = true;
        await Task.Delay((int)_cdTime * 1000);
        _cooldown = false;
        print("TimerTermina");
    }

    
}
