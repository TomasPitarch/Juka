﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HookSkill : MonoBehaviour
{

    [SerializeField]
    HookSkill_SO data;

    HookHead hookHead;

    [SerializeField]
    GameObject skillSpawnPoint;

    bool _cooldown;
    float _cdTime;
    float _skillRange;
    float _skillSpeed;

    bool _hookColition;


    Vector3 HookInitialPosition;
    Vector3 SkillDirection;


    LineRenderer myLine;
    private void Start()
    {

        //Datatake//
        _cdTime = data._coolDownTime;
        _skillRange = data._skillRange;
        _skillSpeed = data._skillSpeed;
        _cooldown = false;
        _hookColition = false;

        //HookHeadCreation and event suscription//
        hookHead = Instantiate(data.hookHead).GetComponent<HookHead>();
        hookHead.OnObjectCollision += HookColitionHanlder;

        //LineRendererHook creation and config//
        myLine = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer; 
        myLine.SetColors(Color.red, Color.red);
        myLine.SetWidth(0.2f, 0.2f);
        myLine.SetPosition(0, Vector3.zero);
        myLine.SetPosition(1, Vector3.zero);
        myLine.material = data.myMaterial;
        myLine.enabled = false;
               
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HookColitionHanlder();
        }
    }
    void HookColitionHanlder()
    {
        _hookColition = true;
    }
    public void CastHook(Vector3 point)
    {
        point.y = skillSpawnPoint.transform.position.y;

        if (!_cooldown)
        {
            SkillDirection = (point - skillSpawnPoint.transform.position).normalized;
            HookBehaviour();
            CoolDownTimer();
        }
    }
    async void CoolDownTimer()
    {
        print("TimerComienza");
        _cooldown = true;
        await Task.Delay((int)_cdTime*1000);
        _cooldown = false;
        print("TimerTermina");
    }

    async void HookBehaviour()
    {
        //print("Hook");
        myLine.enabled = true;

        hookHead.HookHeadActive(skillSpawnPoint.transform.position);
        HookInitialPosition = skillSpawnPoint.transform.position;
        var distance = (HookInitialPosition - hookHead.transform.position).magnitude;

        while (!_hookColition && distance<_skillRange)
        {
            //print("yendo");
            var distanceToMove = SkillDirection * _skillSpeed * Time.deltaTime;
            hookHead.Move(distanceToMove);
            distance = (HookInitialPosition - hookHead.transform.position).magnitude;

            myLine.SetPosition(0,skillSpawnPoint.transform.position);
            myLine.SetPosition(1, hookHead.transform.position);

            await Task.Yield();
        }

        if(_hookColition)
        {
            print("Colision el hook asique se termina");
            hookHead.HookHeadOff();
            _hookColition = false;
            myLine.enabled = false;

            return;
        }


        distance = (skillSpawnPoint.transform.position - hookHead.transform.position).magnitude;

        while (distance > 0.5f)
        {
            //print("volviendo");
            var directionToCome = (skillSpawnPoint.transform.position - hookHead.transform.position).normalized;
            var distanceToMove = directionToCome * _skillSpeed * Time.deltaTime;

            hookHead.Move(distanceToMove);
            distance = (skillSpawnPoint.transform.position - hookHead.transform.position).magnitude;

            myLine.SetPosition(0, skillSpawnPoint.transform.position);
            myLine.SetPosition(1, hookHead.transform.position);

            await Task.Yield();
        }

        hookHead.HookHeadOff();
        myLine.enabled = false;
        //print("retorno completamente");

    }
    
       
}
