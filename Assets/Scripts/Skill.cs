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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected async void CoolDownTimer()
    {

        _cooldown = true;


        var timer = Task.Delay((int)_cdTime * 1000);

        while (!timer.IsCompleted)
        {

            if (!_tokenCoolDownTimer)
            {
                _tokenCoolDownTimer = true;
                return;
            }

            await Task.Yield();
        }

        _cooldown = false;
    }
}
