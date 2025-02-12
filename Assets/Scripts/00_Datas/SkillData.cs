using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SkillData
{
    public Motion skillMotion;
    public int effectId;
    public int attackArea;
    public float attack;
    public float knockback;
    public float knockbackDuration;
}