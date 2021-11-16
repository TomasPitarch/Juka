using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShoot : Skill, IReseteable
{
    [SerializeField]
    Team myTeam;
    public virtual void CastSkillShoot(Vector3 point)
    {
        if(_cooldown)
        {
            return;
        }
    }

    public virtual void ResetCDs()
    {
        print("resteo CD");
        _tokenCoolDownTimer = true;
        _cooldown = false;
    }
}
