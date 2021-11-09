using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using System.Threading.Tasks;
using UnityEngine;

public class HookHead : MonoBehaviourPun
{
    public event Action OnObjectCollision = delegate { };
    public event Action OnHooksEnd = delegate { };
    public event Action<HookHead> OnHooksDestroy = delegate { };

    M CharacterHooked;
    public Team team;

    /// <summary>
    LineRenderer myLine;
    bool _hookColition;

    Vector3 HookInitialPosition;
    Vector3 SkillDirection;

    [SerializeField]
    GameObject skillSpawnPoint;

    float _skillRange;
    float _skillSpeed;
    /// </summary>


    private void Await()
    {
        _hookColition = false;
    }
    public void Init(GameObject SpawnPoint, HookSkill_SO data,Vector3 dir)
    {
        print("inicializo el hook con la direccion:"+dir);

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
        HookBehaviour();
    }

    async void HookBehaviour()
    {
        transform.position = skillSpawnPoint.transform.position;
        var HookInitialPosition = skillSpawnPoint.transform.position;
        var distance = (HookInitialPosition - transform.position).magnitude;

        while (!_hookColition && distance < _skillRange)
        {
            var distanceToMove = SkillDirection * _skillSpeed * Time.deltaTime;
            Move(distanceToMove);
            distance = (HookInitialPosition - transform.position).magnitude;

            myLine.SetPosition(0, skillSpawnPoint.transform.position);
            myLine.SetPosition(1, transform.position);

            await Task.Yield();
        }

        //if(_hookColition)
        //{
        //    print("Colision el hook asique se termina");
        //    hookHead.HookHeadOff();
        //    _hookColition = false;
        //    myLine.enabled = false;

        //    return;
        //}


        distance = (skillSpawnPoint.transform.position - transform.position).magnitude;

        while (distance > 0.5f)
        {
            //print("volviendo");
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
    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag=="Character")
        {
            print("colision de hook con character");

            CharacterHooked = collision.gameObject.GetComponent<M>();
            CharacterHooked.Hooked(this);
           
            
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
