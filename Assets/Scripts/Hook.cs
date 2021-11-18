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

        print("6-Inicializo el hook");

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
        print("liquidamos el hook");
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

                print("7-el hook choca contra un player");
                CharacterHooked = other.gameObject.GetComponent<M>();
                var charPV_ID = CharacterHooked.photonView.ViewID;
                var playerHooked = ServerManager.Instance.GetPlayer(charPV_ID);
                var hookPV_ID = photonView.ViewID;

                int[] IDs = new int[] {hookPV_ID,CasterID};

                print("8-el hook del server le manda un rpc al cliente del character hookeado");
                CharacterHooked.photonView.RPC("Hooked", playerHooked, IDs);

                if (CasterID!=charPV_ID)
                {
                _hookCollided = true;
                }
               
           

                //OnObjectCollision();
        }
        
    }

    [PunRPC]
    void RemoveCharacterFromHook()
    {
        CharacterHooked = null;
    }
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
