using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HookSkillData", menuName = "ScriptableObjects/SkillsData", order = 1)]
public class HookSkill_SO : ScriptableObject
{
    public float _coolDownTime;
    public float _skillRange;
    public float _skillSpeed;
    public GameObject hookHead;
    public Material myMaterial;
}
