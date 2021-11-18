using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class NetSkill : SkillShoot
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
    public override void CastSkillShoot(Vector3 point)
    {
        if (CanSkill())
        {
            SkillShoot_SVRequest(point);
            CoolDownTimer();
        }

    }
    void SkillShoot_SVRequest(Vector3 point)
    {
        photonView.RPC("SkillingNet", RpcTarget.MasterClient, point);
    }

    [PunRPC]
    void SkillingNet(Vector3 point)
    {
        SpawnNet();

        point.y = skillSpawnPoint.transform.position.y;
        SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
        net.Init(skillSpawnPoint.transform.position, SkillDirection, _skillSpeed, _lifeTime, photonView.ViewID);
       
    }

    void SpawnNet()
    {
        //HookHeadCreation and event suscription//
        net = PhotonNetwork.Instantiate("Net",Vector3.zero,Quaternion.identity).GetComponent<Net>();
        net.OnObjectCollision += NetColitionHanlder;
    }

    internal bool CanSkill()
    {
        return !_cooldown;
    }
}
