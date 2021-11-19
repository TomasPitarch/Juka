using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;

public class Hook : MonoBehaviourPun
{
    public event Action OnObjectCollision = delegate { };
    public event Action OnHooksEnd = delegate { };
    public event Action<Hook> OnHooksDestroy = delegate { };

    public M CharacterHooked;

    bool _hookCollided;

    Vector3 HookInitialPosition;
    Vector3 SkillDirection;

    [SerializeField]
    GameObject skillSpawnPoint;

    float _skillRange;
    float _skillSpeed;
    int CasterID;
   

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        _hookCollided = false;
    }
    public void Init(GameObject SpawnPoint, HookSkill_SO data,Vector3 dir,int CasterPv_Id)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }



        CasterID = CasterPv_Id;

        _skillRange = data._skillRange;
        _skillSpeed = data._skillSpeed;
        SkillDirection = dir;
        skillSpawnPoint = SpawnPoint;

        HookBehaviour();
    }
    async void HookBehaviour()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        var HookInitialPosition = skillSpawnPoint.transform.position;
        transform.position = HookInitialPosition;
        
        var distance = (HookInitialPosition - transform.position).magnitude;

        while (!_hookCollided && distance < _skillRange)
        {
            var distanceToMove = SkillDirection * _skillSpeed * Time.deltaTime;
            Move(distanceToMove);
            distance = (HookInitialPosition - transform.position).magnitude;

            await Task.Yield();
        }

        distance = (HookInitialPosition - transform.position).magnitude;

        _hookCollided = true;

        while (distance > 0.5f)
        {
            var directionToCome = (HookInitialPosition - transform.position).normalized;
            var distanceToMove = directionToCome * _skillSpeed * Time.deltaTime;

            Move(distanceToMove);
            distance = (HookInitialPosition - transform.position).magnitude;

            await Task.Yield();
        }

        HookEnds();

    }
    void HookHeadPosition(Vector3 FromActivePosition)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        transform.position = FromActivePosition;
    }
    void HookEnds()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        CharacterHooked = null;
        OnHooksEnd();
        OnHooksDestroy(this);
        PhotonNetwork.Destroy(photonView);
    }
    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (other.gameObject.tag == "Character" && !_hookCollided)
        {

                CharacterHooked = other.gameObject.GetComponent<M>();
                var charPV_ID = CharacterHooked.photonView.ViewID;
                var playerHooked = ServerManager.Instance.GetPlayer(charPV_ID);
                var hookPV_ID = photonView.ViewID;
                var Caster = PhotonView.Find(CasterID).GetComponent<M>();


                if (CharacterHooked.photonView.ViewID == CasterID)
                {
                     print("el hook le pego al dueño, no hago nada");
                     _hookCollided = false;
                     CharacterHooked = null;
                     return;
                }
                else if (CharacterHooked.myTeam == Caster.myTeam)
                {
                    print("mismo equipo,traigo hacia mi");
                    CharacterHooked.photonView.RPC("HookedByAly", playerHooked, hookPV_ID);
                    _hookCollided = true;
                }
                else
                {

                    CharacterHooked.photonView.RPC("HookedByEnemy", playerHooked, CasterID);
                    CharacterHooked = null;
                    _hookCollided = true;
                }

            //OnObjectCollision();
        }
        
    }

    //[PunRPC]
    //void RemoveCharacterFromHook()
    //{
    //    CharacterHooked = null;
    //}
    void Move(Vector3 distanceToMove)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        transform.position += distanceToMove;

        if(CharacterHooked != null)
        {
            var playerHooked= ServerManager.Instance.GetPlayer(CharacterHooked.photonView.ViewID);
            CharacterHooked.photonView.RPC("HookedMove",playerHooked, distanceToMove);
        }
    }
    
}
