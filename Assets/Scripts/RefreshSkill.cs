using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshSkill : Skill
{
    [SerializeField]
    int skillCost;

    IReseteable[] ListOfSkills;
    private void Start()
    {
        ListOfSkills = GetComponents<IReseteable>();
    }
    public void Cast(GoldComponent gold)
    {
        if (!_onCooldown && gold.CanPay(skillCost))
        {
            gold.Pay(skillCost);
            Rearm();
        }
    }

    public void Rearm()
    {
        foreach (var skill in ListOfSkills)
        {
            skill.ResetCDs();
        }
    }
}
