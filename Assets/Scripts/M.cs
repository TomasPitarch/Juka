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
    public event Action OnHooked = delegate { };
    public event Action OnIdle = delegate { };
    public event Action OnTrapped = delegate { };
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

        NormalActionStatus();
    }

    // States//
    void NormalActionStatus()
    {
        _canMove = true;
        _canSkill1 = true;
        _canSkill2 = true;
        _canSkill3 = true;
        OnIdle();

        GetComponent<Rigidbody>().detectCollisions = true;
    }
    void NetActionStatus()
    {
        myNavMesh.ResetPath();
        OnTrapped();
        _canMove = false;
        _canSkill1 = false;
        _canSkill2 = false;
        _canSkill3 = false;
    }
    void HookedActionStatus()
    {
        myNavMesh.ResetPath();
        OnHooked();
        _canMove = false;
        _canSkill1 = false;
        _canSkill2 = false;
        _canSkill3 = false;
    }
    void ShiftActionStatus()
    {
        myNavMesh.ResetPath();
        _canMove = false;
        _canSkill1 = false;
        _canSkill2 = false;
        _canSkill3 = false;

        
    }
   
    void DieActionStatus()
    {

        print("14-Die status");
        myNavMesh.ResetPath();
        _canMove = false;
        _canSkill1 = false;
        _canSkill2 = false;
        _canSkill3 = false;

    }
    void GhostForm()
    {
        OnGhostStart();
        ShiftActionStatus();
    }
    void GhostFormEnd()
    {
        if (photonView.IsMine)
        {
            print("termine mi forma de shift");
            OnGhostEnd();
            NormalActionStatus();
        }
        else
        {
            print("este shift no soy el dueño");
        }
    }

    //----------RPCs------------//
    //[PunRPC]
    //public void Hooked(int[] IDs)
    //{
    //    var HookID = IDs[0];
    //    var hookCasterID = IDs[1];


    //    if (photonView.ViewID == hookCasterID)
    //    {
    //        var hook = PhotonView.Find(HookID).GetComponent<Hook>();
    //        hook.photonView.RPC("RemoveCharacterFromHook", RpcTarget.MasterClient);
    //        return;
    //    }

    //    var CasterChar = PhotonView.Find(hookCasterID);
    //    var CasterTeam = CasterChar.GetComponent<M>().myTeam;


    //    if (CasterTeam != myTeam)
    //    {
            
    //        Die(hookCasterID);

    //        //Como muero me saco del gancho//
    //        var hook = PhotonView.Find(HookID).GetComponent<Hook>();
    //        hook.photonView.RPC("RemoveCharacterFromHook", RpcTarget.MasterClient);
    //    }
    //    else
    //    {
    //        Catched(HookID);
    //    }
    //}

    [PunRPC]
    void HookedByEnemy(int hookCasterID)
    {
        Die(hookCasterID);
    }
    [PunRPC]
    void HookedByAly(int HookID)
    {
        Catched(HookID);
    }

    [PunRPC]
    void BackToNormality()
    {
        NormalActionStatus();
    }
    [PunRPC]
    void GetGoldForKill(int goldReward)
    {
        myGold.AddGold(goldReward);
    }
    [PunRPC]
    public void CatchedByNet(int CasterID)
    {
        //Animacion de la Red//
        var net = PhotonNetwork.Instantiate("NetStatus Team 1", transform.position, Quaternion.identity).GetComponent<NetStatus>();
        net.OnNetReleased += NormalActionStatus;
        NetActionStatus();
    }
    [PunRPC]
    void HookedMove(Vector3 distance)
    {
        print("hooked move");
        transform.position += distance;
    }

    //-------Server RPCs-------//
    [PunRPC]
    void ServerCatched(int HookID)
    {
        var hook = PhotonView.Find(HookID).GetComponent<Hook>();
        hook.OnHooksEnd += BackOwnerNormality;
    }
    [PunRPC]
    public void ServerDie()
    {
        ServerDieStatus();
    }
    [PunRPC]
    public void ServerRespawn()
    {
        ServerRespawnStatus();
    }

    //------Server Requests-----//
    private void BringGoldForKill_Request(int hookCasterID)
    {
        ServerManager.Instance.photonView.RPC("GoldToKiller", RpcTarget.MasterClient, hookCasterID);
    }

    //------Server-----//
    void ServerDieStatus()
    {
        print("13-Se puso para que no colisione");
        GetComponent<Rigidbody>().detectCollisions = false;
    }
    void ServerRespawnStatus()
    {
        GetComponent<Rigidbody>().detectCollisions = true;
    }
    void BackOwnerNormality()
    {
        var owner = ServerManager.Instance.GetPlayer(photonView.ViewID);
        photonView.RPC("BackToNormality", owner);
    }

    //-------ASync-------//
    async void Respawining()
    {
        await Task.Delay(5000);
        Respawn();
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
        
    }
    public void Skill1(Vector3 point)
    {
        if (_canSkill1 && hookSkill.CanSkill())
        {

            myNavMesh.ResetPath();
            TurnToCastDirection(point);
            hookSkill.CastSkillShoot(point);

            OnHookShoot();
        }
    }
    public void Skill2(Vector3 point)
    {

        if (_canSkill2 && netSkill.CanSkill())
        {
            myNavMesh.ResetPath();
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
    void Die(int killerID)
    {
        photonView.RPC("ServerDie", RpcTarget.MasterClient);

        DieActionStatus();
        OnDie();

        ServerManager.Instance.photonView.RPC("CharacterDie_Request", RpcTarget.MasterClient, photonView.ViewID);
        BringGoldForKill_Request(killerID);
        Respawining();
    }
    void Catched(int HookID)
    {
        HookedActionStatus();
        photonView.RPC("ServerCatched", RpcTarget.MasterClient,HookID);
        
    }
    public void Respawn()
    {
        gameObject.SetActive(true);
        var respawnPosition= ServerManager.Instance.ClientManager.GetRandomReSpawnPoint(myTeam).position;
        myNavMesh.Warp(respawnPosition);
        
        OnRespawn();

        NormalActionStatus();

        photonView.RPC("ServerRespawn", RpcTarget.MasterClient);
    }
   

    
}
