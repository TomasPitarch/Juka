using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum Team{A,B};
public class M : MonoBehaviour
{
    public event Action OnMove = delegate { };
    public event Action OnStop = delegate { };
    public event Action OnHookShoot = delegate { };
    public event Action OnNetShoot = delegate { };
    public event Action OnDie = delegate { };
    public event Action OnRespawn = delegate { };



    bool _canMove = true;
    bool _canSkill1=true;
    bool _canSkill2 = true;
    bool _canSkill3 = true;

    [SerializeField]
    public Team myTeam;

    [SerializeField]
    NavMeshAgent myNavMesh;

    [SerializeField]
    HookSkill hookSkill;

    [SerializeField]
    NetSkill netSkill;

    [SerializeField]
    ShiftSkill shiftSkill;

    [SerializeField]
    RefreshSkill refreshSkill;

    List<IReseteable> ListOfSkills;

    GoldComponent myGold;
    void Start()
    {
        myNavMesh = GetComponent<NavMeshAgent>();
        hookSkill = GetComponent<HookSkill>();
        netSkill = GetComponent<NetSkill>();

        shiftSkill = GetComponent<ShiftSkill>();
        shiftSkill.OnShiftStart += GhostForm;
        shiftSkill.OnShiftEnd += GhostFormEnd;


        refreshSkill = GetComponent<RefreshSkill>();


        myGold = GetComponent<GoldComponent>();

       

        ListOfSkills = new List<IReseteable>();

        ListOfSkills.Add(hookSkill);
        ListOfSkills.Add(netSkill);
        ListOfSkills.Add(shiftSkill);




        NormalStatus();
    }

    internal void Skill4()
    {
        refreshSkill.Cast(myGold,this);
    }

    public void Skill3()
    {
        shiftSkill.CastShift();
    }

    void GhostFormEnd()
    {
        GetComponent<MeshRenderer>().enabled = true;
        NormalStatus();
    }

    void GhostForm()
    {
        GetComponent<MeshRenderer>().enabled = false;
        ShiftStatus();
    }

    public void Move(Vector3 destination)
    {
        if(_canMove)
        {
            myNavMesh.SetDestination(destination);
            OnMove();
        }
       
    }
    public void StopMove()
    {
        myNavMesh.isStopped=true;

        OnStop();
    }

    public void Skill1(Vector3 point)
    {
        if (_canSkill1)
        {
            hookSkill.CastHook(point);

            OnHookShoot();
        }

    }

    public void Skill2(Vector3 point)
    {
        if (_canSkill2)
        {
            netSkill.CastNet(point);

            OnNetShoot();
        }

    }

    public void Hooked(HookHead Hook)
    {
        if(Hook.team!=myTeam)
        {
            Die();
            Respawining();
        }
        else
        {
            Catched(Hook);
        }
    }

    private void Catched(HookHead hook)
    {
        NetStatus();
        hook.OnHooksEnd += BackToNormality;
    }
    

    private void BackToNormality()
    {
        NormalStatus();
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
    
    void NetStatus()
    {
         myNavMesh.ResetPath();
         _canMove = false;
         _canSkill1 = false;
         _canSkill2 = false;
         _canSkill3 = false;
    }
    void ShiftStatus()
    {
        myNavMesh.ResetPath();
        _canMove = false;
        _canSkill1 = false;
        _canSkill2 = false;
        _canSkill3 = false;
    }
    public void CatchedByNet(GameObject netStatusPrefab)
    {
        var net= Instantiate(netStatusPrefab).GetComponent<NetStatus>();
        net.Init(transform.position);
        net.OnNetReleased += NormalStatus;
        NetStatus();
        
    }

    void NormalStatus()
    {
        _canMove = true;
        _canSkill1 = true;
        _canSkill2 = true;
        _canSkill3 = true;

    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        OnRespawn();
    }
    public void Rearm()
    {
        foreach(var skill in ListOfSkills)
        {
            skill.ResetCDs();
        }
    }

    
}
