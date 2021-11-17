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
    public event Action<Vector3> OnMove = delegate { };
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

    [SerializeField]
    GameObject _cursorArrowPrefab;

    Animation cursorAnimation;

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


        cursorAnimation = Instantiate(_cursorArrowPrefab, new Vector3(100,100,100),Quaternion.identity).GetComponent<Animation>();

        OnMove += ShowMovementCursor;

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
    void DieStatus()
    {

        print("Die Status");

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

            OnMove(destination);
        }
       
    }

    void ShowMovementCursor(Vector3 destination)
    {
        cursorAnimation.transform.position = destination;
        cursorAnimation.Rewind();
        cursorAnimation.Play();
    }

    public void StopMove()
    {
        //myNavMesh.isStopped=true;
        myNavMesh.ResetPath();
        //myNavMesh.enabled = false;
        OnStop();
    }
    public void Skill1(Vector3 point)
    {
        if (_canSkill1 && hookSkill.CanSkill())
        {
            TurnToCastDirection(point);
            hookSkill.CastSkillShoot(point);

            OnHookShoot();
        }

    }
    public void Skill2(Vector3 point)
    {

        if (_canSkill2 && netSkill.CanSkill())
        {
            TurnToCastDirection(point);
            netSkill.CastSkillShoot(point);

            OnNetShoot();
        }

    }
    public void Skill3()
    {
        if (_canSkill3 && shiftSkill.CanSkill())
        {
            shiftSkill.CastShift();
        }
            
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
        StopMove();

        var HookID = IDs[0];
        var hookCasterID = IDs[1];



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
            NormalStatus();
            return;
        }

       

        var CasterChar = PhotonView.Find(hookCasterID);
        var CasterTeam = CasterChar.GetComponent<M>().myTeam;


        if (CasterTeam != myTeam)
        {
            Die(hookCasterID);
        }
        else
        {
            print("Me Hookeo un aliado");
            Catched(Hook);
        }
    }
    void Die(int killerID)
    {
        photonView.RPC("ServerDie", RpcTarget.MasterClient);
        OnDie();

        ServerManager.Instance.photonView.RPC("CharacterDie_Request", RpcTarget.MasterClient, photonView.ViewID);
        BringGoldForKill_Request(killerID);
        Respawining();
    }
    private void BringGoldForKill_Request(int hookCasterID)
    {
        ServerManager.Instance.photonView.RPC("GoldToKiller",RpcTarget.MasterClient,hookCasterID);
    }

    [PunRPC]
    void GetGoldForKill(int goldReward)
    {
        myGold.AddGold(goldReward);
    }
    private void Catched(Hook hook)
    {
        print("Catched");
        NetStatus();
        hook.OnHooksEnd += BackToNormality;
    }

    [PunRPC]
    public void ServerDie()
    {
        ShiftStatus();
    }
    [PunRPC]
    public void ServerRespawn()
    {
        NormalStatus();
    }
    async void Respawining()
    {
        await Task.Delay(5000);
        Respawn();
    }

    [PunRPC]
    public void CatchedByNet(int CasterID)
    {
        //print("Soy el atrapado");

        if (photonView.ViewID == CasterID)
        {
            //print("Me pegue la red, no hago nada");
            return;
        }
        ///////////////////////////
        var net = PhotonNetwork.Instantiate("NetStatus Team 1",transform.position,Quaternion.identity).GetComponent<NetStatus>();
        net.OnNetReleased += NormalStatus;
        NetStatus();
        
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        var respawnPosition= ServerManager.Instance.ClientManager.GetRandomReSpawnPoint(myTeam).position;
        myNavMesh.Warp(respawnPosition);
        
        OnRespawn();

        NormalStatus();
        photonView.RPC("ServerRespawn", RpcTarget.MasterClient);
    }
   

    
}
