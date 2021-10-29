using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShiftSkill : Skill, IReseteable
{
    public event Action OnShiftStart = delegate { };
    public event Action OnShiftEnd = delegate { };

    [SerializeField]
    float shiftTime;
    

    public void CastShift()
    {
        if(!_cooldown)
        {
            print("Shift");
            ShiftBehaviour();
            CoolDownTimer();
        }
    }
   
    async void ShiftBehaviour()
    {
        OnShiftStart();
        await Task.Delay((int)shiftTime*1000);
        OnShiftEnd();
    }

    public void ResetCDs()
    {
        _cooldown = false;
    }
}
