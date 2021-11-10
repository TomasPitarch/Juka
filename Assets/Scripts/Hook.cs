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
    //public Team team;

    /// <summary>
    LineRenderer myLine;
    bool _hookCollided;

    Vector3 HookInitialPosition;
    Vector3 SkillDirection;

    [SerializeField]
    GameObject skillSpawnPoint;

    float _skillRange;
    float _skillSpeed;
    int CasterID;
    /// </summary>


    private void Await()
    {
        _hookCollided = false;
    }
    public void Init(GameObject SpawnPoint, HookSkill_SO data,Vector3 dir,int CasterPv_Id)
    {

        CasterID = CasterPv_Id;

        _skillRange = data._skillRange;
        _skillSpeed = data._skillSpeed;

        //LineRendererHook creation and config//
        myLine = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        myLine.SetColors(Color.red, Color.red);
        myLine.SetWidth(0.2f, 0.2f);
        myLine.SetPosition(0, Vector3.zero);
        myLine.SetPosition(1, Vector3.zero);
        myLine.material = data.myMaterial;

        SkillDirection = dir;
        skillSpawnPoint = SpawnPoint;

        print("el caster del hook es:" + ServerManager.Instance.GetPlayer(CasterID));

        HookBehaviour();
    }
    async void HookBehaviour()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        transform.position = skillSpawnPoint.transform.position;
        var HookInitialPosition = skillSpawnPoint.transform.position;
        var distance = (HookInitialPosition - transform.position).magnitude;

        while (!_hookCollided && distance < _skillRange)
        {
            var distanceToMove = SkillDirection * _skillSpeed * Time.deltaTime;
            Move(distanceToMove);
            distance = (HookInitialPosition - transform.position).magnitude;

            myLine.SetPosition(0, skillSpawnPoint.transform.position);
            myLine.SetPosition(1, transform.position);

            await Task.Yield();
        }

        distance = (skillSpawnPoint.transform.position - transform.position).magnitude;

        _hookCollided = true;

        while (distance > 0.5f)
        {
            var directionToCome = (skillSpawnPoint.transform.position - transform.position).normalized;
            var distanceToMove = directionToCome * _skillSpeed * Time.deltaTime;

            Move(distanceToMove);
            distance = (skillSpawnPoint.transform.position - transform.position).magnitude;

            myLine.SetPosition(0, skillSpawnPoint.transform.position);
            myLine.SetPosition(1, transform.position);

            await Task.Yield();
        }

        HookEnds();

    }
    public void HookHeadPosition(Vector3 FromActivePosition)
    {
        transform.position = FromActivePosition;
    }
    public void HookEnds()
    {
        CharacterHooked = null;
        OnHooksEnd();
        OnHooksDestroy(this);
        PhotonNetwork.Destroy(photonView);
    }
    private void OnTriggerEnter(Collider other)
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

                int[] IDs = new int[] {hookPV_ID,CasterID};
                CharacterHooked.photonView.RPC("Hooked", playerHooked, IDs);

                _hookCollided = true;

                OnObjectCollision();
            }
        
    }
    
    public void Move(Vector3 distanceToMove)
    {
        transform.position += distanceToMove;

        if(CharacterHooked)
        {
            CharacterHooked.transform.position += distanceToMove;
        }
    }
    
}
