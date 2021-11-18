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
        print("11-hooked action status, reseteo movilidad en navmesh y desactivo los skilles y movimiento");
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
        OnGhostEnd();
        NormalActionStatus();
    }

    //----------RPCs------------//
    [PunRPC]
    public void Hooked(int[] IDs)
    {
        print("9-Me hookearon");
        var HookID = IDs[0];
        var hookCasterID = IDs[1];


        if (photonView.ViewID == hookCasterID)
        {
            print("me autohookie, me saco del gancho");
            var hook = PhotonView.Find(HookID).GetComponent<Hook>();
            hook.photonView.RPC("RemoveCharacterFromHook", RpcTarget.MasterClient);
            return;
        }

        var CasterChar = PhotonView.Find(hookCasterID);
        var CasterTeam = CasterChar.GetComponent<M>().myTeam;


        if (CasterTeam != myTeam)
        {
            print("10-Me hookeo un enemigo");
            
            Die(hookCasterID);

            //Como muero me saco del gancho//
            var hook = PhotonView.Find(HookID).GetComponent<Hook>();
            hook.photonView.RPC("RemoveCharacterFromHook", RpcTarget.MasterClient);
        }
        else
        {
            print("10-Me Hookeo un aliado");
            Catched(HookID);
        }
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
        print("13-le llega al servidor que fui atrapado, y le avisa quien atrapo, y se suscribe");

        var hook = PhotonView.Find(HookID).GetComponent<Hook>();
        hook.OnHooksEnd += BackOwnerNormality;
    }
    [PunRPC]
    public void ServerDie()
    {
        print("12-Le llega al servidor que murio");
        ServerDieStatus();
    }
    [PunRPC]
    public void ServerRespawn()
    {
        print("19-Le llega al servidor ya respawnie asique puedo volver a colisionar");
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
        print("20-Se puso para que  colisione");
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

        print("15-Me pogno a respawnear en 5 segundos");
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
            print("1- el character Skillea el hook");
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
        print("11-Muero y le aviso al server que cambie su estatus a no colisionable");
        photonView.RPC("ServerDie", RpcTarget.MasterClient);

        print("13-Me pongo en estado de muerte y no puedo ejecutar acciones");
        DieActionStatus();
        OnDie();

        ServerManager.Instance.photonView.RPC("CharacterDie_Request", RpcTarget.MasterClient, photonView.ViewID);
        BringGoldForKill_Request(killerID);
        Respawining();
    }
    void Catched(int HookID)
    {
        print("11-Catched");
        HookedActionStatus();
        print("12-le aviso al servidor que fui atrapado");

        photonView.RPC("ServerCatched", RpcTarget.MasterClient,HookID);
        
    }
    public void Respawn()
    {

        print("16-Respawneo");
        gameObject.SetActive(true);
        var respawnPosition= ServerManager.Instance.ClientManager.GetRandomReSpawnPoint(myTeam).position;
        myNavMesh.Warp(respawnPosition);
        
        OnRespawn();

        NormalActionStatus();

        print("17-Le aviso al servidor que respawnieo y puede volver a colisionar");
        photonView.RPC("ServerRespawn", RpcTarget.MasterClient);
    }
   

    
}
