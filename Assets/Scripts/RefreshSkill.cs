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
        print("bool de cd del rearm cast:" + _cooldown);
        print("oro:" + gold.CanPay(skillCost));

        if (!_cooldown && gold.CanPay(skillCost))
        {
            print("Rearm");
            gold.Pay(skillCost);
            Rearm();
        }
    }

    public void Rearm()
    {
        print("rearm cantidad:" + ListOfSkills.Length);
        foreach (var skill in ListOfSkills)
        {
            skill.ResetCDs();
        }
    }
}
