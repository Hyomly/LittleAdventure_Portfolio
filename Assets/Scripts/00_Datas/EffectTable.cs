using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTable : SingletonMonobehaviour<EffectTable>
{
    public class Data
    {
        public int Id;
        public string Prefab;
    }
    public Dictionary<int, Data> m_effectTable = new Dictionary<int, Data>();
    public Data GetEffectData(int id)
    {
        return m_effectTable[id];
    }
    public void LoadData()
    {
        m_effectTable.Clear();
        TableLoader.Instance.LoadTable("Effect");
        for (int i = 0; i < TableLoader.Instance.Count; i++)
        {
            Data data = new Data();
            data.Id = TableLoader.Instance.GetInteger("Id", i);
            data.Prefab = TableLoader.Instance.GetString("Prefab", i);

            m_effectTable.Add(data.Id, data);
        }
        TableLoader.Instance.Clear();
    }

}
