using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTable : SingletonMonobehaviour<MonsterTable>
{
    public Dictionary<int, Status> m_tableStatus = new Dictionary<int, Status>();

    public void LoadStatus()
    {
        m_tableStatus.Clear();
        TableLoader.Instance.LoadTable("Monsters");
        for (int i = 0; i < TableLoader.Instance.Count; i++)
        {
            Status status = new Status();
            status.num = TableLoader.Instance.GetInteger("Num", i);
            status.hpMax = TableLoader.Instance.GetInteger("HpMax", i);
            status.hp = TableLoader.Instance.GetInteger("Hp", i);
            status.attack = TableLoader.Instance.GetInteger("Attack", i);
            m_tableStatus.Add(status.num, status);
        }
        TableLoader.Instance.Clear();
    }
    public Status GetStatusData(int index)
    {
        return m_tableStatus[index];    
    }
    protected override void OnAwake()
    {
        LoadStatus();
    }
}
