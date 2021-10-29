using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshSkill : Skill
{
    [SerializeField]
    int skillCost;

    public void Cast(GoldComponent gold,M character)
    {
        if(!_cooldown && gold.CanPay(skillCost))
        {
            print("Rearm");
            gold.Pay(skillCost);

            character.Rearm();
        }

    }
    


}
