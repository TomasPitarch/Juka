using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_View : MonoBehaviour
{
    [SerializeField]
    Image Q_skill;

    [SerializeField]
    Image W_skill;

    [SerializeField]
    Image E_skill;

    [SerializeField]
    Image R_skill;


    [SerializeField]
    TextMeshProUGUI goldAmount;

    


    public void SetHandlers(M model)
    {
        model.GetComponent<HookSkill>().OnCoolDownUpdate+=Q_Skill_UIHandler;

        model.GetComponent<NetSkill>().OnCoolDownUpdate += W_Skill_UIHandler;

        model.GetComponent<ShiftSkill>().OnCoolDownUpdate += E_Skill_UIHandler;

        //model.GetComponent<RefreshSkill>().OnCoolDownUpdate += R_Skill_UIHandler;

        model.GetComponent<GoldComponent>().OnGoldUpdate+=GoldUpdate_UIHandler;

    }

    void Q_Skill_UIHandler(float actual, float total)
    {

        Q_skill.fillAmount = actual / total;
    }
    void W_Skill_UIHandler(float actual, float total)
    {
        W_skill.fillAmount = actual / total;
    }
    void E_Skill_UIHandler(float actual, float total)
    {

        E_skill.fillAmount = actual / total;
    }
    void R_Skill_UIHandler()
    {

    }

    void GoldUpdate_UIHandler(int gold)
    {
        goldAmount.text = gold.ToString();
    }
}
