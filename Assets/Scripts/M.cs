using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public enum Team{A,B};
public class M : MonoBehaviourPun
{
    public event Action OnMove = delegate { };
    public event Action OnStop = delegate { };
    public event Action OnHookShoot = delegate { };
    public event Action OnNetShoot = delegate { };
    public event Action OnDie = delegate { };
    public event Action OnRespawn = delegate { };

    public event Action OnGhostStart = delegate { };
    public event Action OnGhostEnd = delegate { };


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

   

    GoldComponent myGold;

    [SerializeField]
    GameObject netStatusPrefab;
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


        NormalStatus();
    }


    // States//
    void NormalStatus()
    {
        _canMove = true;
        _canSkill1 = true;
        _canSkill2 = true;
        _canSkill3 = true;



        GetComponent<Rigidbody>().detectCollisions = true;
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

        GetComponent<Rigidbody>().detectCollisions = false;
    }
    private void BackToNormality()
    {
        NormalStatus();
    }
    void GhostForm()
    {
        OnGhostStart();
        ShiftStatus();
    }
    void GhostFormEnd()
    {
        OnGhostEnd();
        NormalStatus();
    }

    //Actions//
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
            hookSkill.CastSkillShoot(point);
            TurnToCastDirection(point);

            OnHookShoot();
        }

    }
    public void Skill2(Vector3 point)
    {

        if (_canSkill2)
        {
            netSkill.CastSkillShoot(point);
            TurnToCastDirection(point);


            OnNetShoot();
        }

    }
    public void Skill3()
    {
        shiftSkill.CastShift();
    }
    public void Skill4()
    {
        refreshSkill.Cast(myGold);
    }
    private void TurnToCastDirection(Vector3 point)
    {
        var NormalizedObjectivePosition = new Vector3(point.x,
                                                      transform.position.y,
                                                      point.z);
        transform.LookAt(NormalizedObjectivePosition);
    }

    [PunRPC]
    public void Hooked(int[] IDs)
    {
        var HookID = IDs[0];
        var hookCasterID = IDs[1];

        print("Soy el hookeado");

        var Hook_PV = PhotonView.Find(HookID);


        if (Hook_PV == null)
        {
            print("No encontre el componente photonview del hook por el ID");
            return;
        }


        var Hook = Hook_PV.gameObject.GetComponent<Hook>();

        if (Hook==null)
        {
            print("No encontre el componente hook");
            return;
        }

        if(Hook.CharacterHooked==null)
        {
            print("el hook no tiene a nadie en character hooked");
        }

        if (photonView.ViewID == hookCasterID)
        {
            print("Me autohookie, no hago nada");
            return;
        }
        var CasterChar = PhotonView.Find(hookCasterID);
        var CasterTeam = CasterChar.GetComponent<M>().myTeam;


        if (CasterTeam != myTeam)
        {
            print("Me Hookeo un enemigo");
            Die();
            print("Mori, entonces aviso que tengo que dar oro");
            BringGoldForKill_Request(hookCasterID);

            Respawining();
        }
        else
        {
            print("Me Hookeo un aliado");
            Catched(Hook);
        }
    }

    private void BringGoldForKill_Request(int hookCasterID)
    {
        print("hago la request de dar oro al servidor");
        ServerManager.Instance.photonView.RPC("GoldToKiller",RpcTarget.MasterClient,hookCasterID);
    }

    [PunRPC]
    void GetGoldForKill(int goldReward)
    {
        print("aca recibiendo oro:" + goldReward);
        myGold.AddGold(goldReward);
    }
    private void Catched(Hook hook)
    {
        print("Catched");
        NetStatus();
        hook.OnHooksEnd += BackToNormality;
    }
    public void Die()
    {
        print("Die");

        ServerManager.Instance.photonView.RPC("CharacterDie_Request",RpcTarget.MasterClient,photonView.ViewID);
        OnDie();

        NetStatus();
    }
    async void Respawining()
    {
        await Task.Delay(5000);
        Respawn();
    }

    [PunRPC]
    public void CatchedByNet(int CasterID)
    {
        print("Soy el atrapado");

        if (photonView.ViewID == CasterID)
        {
            print("Me pegue la red, no hago nada");
            return;
        }
        ///////////////////////////
        var net = PhotonNetwork.Instantiate("NetStatus Team 1",transform.position,Quaternion.identity).GetComponent<NetStatus>();
        net.OnNetReleased += NormalStatus;
        NetStatus();
        
    }

    public void Respawn()
    {
        transform.position=ServerManager.Instance.ClientManager.GetRandomReSpawnPoint(myTeam).position;
        gameObject.SetActive(true);
        OnRespawn();

        NormalStatus();
    }
   

    
}
