using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTable : SingletonMonobehaviour<SkillTable>
{
    public Dictionary<Motion,SkillData>  m_table = new Dictionary<Motion,SkillData>();
    public SkillData GetSkillData(Motion motion)
    {
        return m_table[motion];
    }
    void LoadData()
    {
        TableLoader.Instance.LoadTable("Skill");
        for (int i = 0; i < TableLoader.Instance.Count; i++)
        {
            SkillData data = new SkillData();
            data.skillMotion = TableLoader.Instance.GetEnum<Motion>("Skill_Motion",i);
            data.effectId = TableLoader.Instance.GetInteger("EffectId", i);
            data.attackArea = TableLoader.Instance.GetInteger("AttackArea", i);
            data.attack = TableLoader.Instance.GetFloat("Attack", i);
            data.knockback = TableLoader.Instance.GetFloat("Knockback", i);
            data.knockbackDuration = TableLoader.Instance.GetFloat("KnockbackDuration", i);
            m_table.Add(data.skillMotion, data);
        }
        TableLoader.Instance.Clear();
    }
    protected override void OnStart()
    {
        LoadData();
    }
}
