using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShoot : Skill, IReseteable
{
    [SerializeField]
    Team myTeam;
    public virtual void CastSkillShoot(Vector3 point)
    {
        if(_onCooldown)
        {
            return;
        }
    }

    public virtual void ResetCDs()
    {
        _tokenCoolDownTimer = true;
        _onCooldown = false;
        
    }
}
