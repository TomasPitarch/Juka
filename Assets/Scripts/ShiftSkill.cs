using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Photon.Pun;
using System;
using UnityEngine;

public class ShiftSkill : Skill, IReseteable
{
    public event Action OnShiftStart = delegate { };
    public event Action OnShiftEnd = delegate { };

    [SerializeField]
    float shiftTime;


    private void Start()
    {
        _cdTime = 5f;
    }
    public void CastShift()
    {
        if(_cooldown)
        {
            print("skill en cd");
            return;
        }

        photonView.RPC("ShiftBehaviour", RpcTarget.MasterClient);
        ShiftBehaviour();

        CoolDownTimer();
    }
   
    [PunRPC]
    async void ShiftBehaviour()
    {
        OnShiftStart();
        await Task.Delay((int)shiftTime*1000);
        OnShiftEnd();
    }

    public void ResetCDs()
    {
        _cooldown = false;

        _tokenCoolDownTimer = false;
    }

    
    public bool CanSkill()
    {
        return !_cooldown;
    }
   
}
