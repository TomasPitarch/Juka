using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    public event Action<float,float> OnCoolDownUpdate;
    protected bool _cooldown;
    protected float _cdTime;
    protected bool _tokenCoolDownTimer = false;


    protected async void CoolDownTimer()
    {

        _cooldown = true;


        var timer = Task.Delay((int)_cdTime * 1000);
        var amount = 0f;

        while (!timer.IsCompleted)
        {
            amount += Time.deltaTime;

            if (_tokenCoolDownTimer)
            {
                print("token activado");
                _tokenCoolDownTimer = false;

                OnCoolDownUpdate(_cdTime,_cdTime);

                return;
            }

            OnCoolDownUpdate(amount, _cdTime);
            await Task.Yield();
        }

        OnCoolDownUpdate(_cdTime, _cdTime);

        _cooldown = false;
    }
}
