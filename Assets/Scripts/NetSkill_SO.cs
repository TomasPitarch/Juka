using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetSkillData", menuName = "ScriptableObjects/NetSkillsData", order = 1)]
public class NetSkill_SO : ScriptableObject
{
    public float _coolDownTime;
    public float _lifeTime;
    public float _skillSpeed;
    public GameObject Net;
}
