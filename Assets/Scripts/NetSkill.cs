using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetSkill : Skill, IReseteable
{

    [SerializeField]
    NetSkill_SO data;

    Net net;

    [SerializeField]
    GameObject skillSpawnPoint;

   
    float _lifeTime;
    float _skillSpeed;

    bool _netColition;
    Vector3 SkillDirection;
    private void Start()
    {

        //Datatake//
        _cdTime = data._coolDownTime;
        _lifeTime = data._lifeTime;
        _skillSpeed = data._skillSpeed;
        _cooldown = false;
        _netColition = false;

    }
   
    void NetColitionHanlder()
    {
        _netColition = true;
    }
    public void CastNet(Vector3 point)
    {
        if (!_cooldown)
        {
            print("Net");
            SpawnNet();

            point.y = skillSpawnPoint.transform.position.y;
            SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
            net.Init(skillSpawnPoint.transform.position, SkillDirection, _skillSpeed,_lifeTime);
            CoolDownTimer();
        }
    }
    
    void SpawnNet()
    {
        //HookHeadCreation and event suscription//
        net = Instantiate(data.Net).GetComponent<Net>();
        net.OnObjectCollision += NetColitionHanlder;
    }

    public void ResetCDs()
    {
        _cooldown = false;
    }
}
