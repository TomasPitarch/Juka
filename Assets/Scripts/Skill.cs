using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    protected bool _cooldown;
    protected float _cdTime;
    protected bool _tokenCoolDownTimer = false;


    protected async void CoolDownTimer()
    {
        print("arranca el cd");
        _cooldown = true;


        var timer = Task.Delay((int)_cdTime * 1000);

        while (!timer.IsCompleted)
        {

            if (_tokenCoolDownTimer)
            {
                print("token activado");
                _tokenCoolDownTimer = true;
                return;
            }

            await Task.Yield();
        }


        _cooldown = false;
    }
}
