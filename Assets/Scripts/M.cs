using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class M : MonoBehaviour
{
    public event Action OnMove = delegate { };
    public event Action OnStop = delegate { };
    public event Action OnHookShoot = delegate { };
    public event Action OnNetShoot = delegate { };
    public event Action OnDie = delegate { };
    public event Action OnRespawn = delegate { };


    [SerializeField]
    NavMeshAgent myNavMesh;

    [SerializeField]
    HookSkill hookSkill;

    [SerializeField]
    NetSkill netSkill;
    void Start()
    {
        myNavMesh = GetComponent<NavMeshAgent>();
        hookSkill = GetComponent<HookSkill>();
        netSkill = GetComponent<NetSkill>();
    }


    public void Move(Vector3 destination)
    {
        myNavMesh.SetDestination(destination);
        OnMove();
    }
    public void StopMove()
    {
        myNavMesh.isStopped=true;

        OnStop();
    }

    public void SkillShoot1(Vector3 point)
    {
        hookSkill.CastHook(point);

        OnHookShoot();
    }

    public void SkillShoot2(Vector3 point)
    {
        netSkill.CastNet(point);

        OnNetShoot();
    }

    public void Hooked()
    {
        Die();
        Respawining();
    }
    public void Die()
    {
        OnDie();
        gameObject.SetActive(false);
        
    }
    async void Respawining()
    {
        await Task.Delay(5000);
        Respawn();
    }
    public void Respawn()
    {
        gameObject.SetActive(true);
        OnRespawn();
    }


    
}
