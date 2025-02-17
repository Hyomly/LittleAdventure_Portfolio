using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectPool : SingletonMonobehaviour<EffectPool>
{
    [SerializeField]
    int m_presetSize = 1; //몇개씩 pool 만들것인지
    [SerializeField]
    List<string> m_effectNameList = new List<string>();
    Dictionary<string, GameObjectPool<EffectPoolUnit>> m_effectPool = new Dictionary<string, GameObjectPool<EffectPoolUnit>>();
    Dictionary<string, GameObject> m_prefabList = new Dictionary<string, GameObject>();

    public EffectPoolUnit Create(string effectName)
    {
        return Create(effectName, Vector3.zero, Quaternion.identity);
    }
    public EffectPoolUnit Create(string effectName, Vector3 position, Quaternion rotation)
    {
        EffectPoolUnit poolUnit = null;
        GameObjectPool<EffectPoolUnit> pool = null;
        m_effectPool.TryGetValue(effectName, out pool);
        if (pool == null) return null;

        for (int i = 0; i < pool.Count; i++)
        {
            poolUnit = pool.Get();
            if (!poolUnit.IsReady)
            {
                pool.Set(poolUnit);
                poolUnit = null;
            }
            else
                break;
        }
        if (poolUnit == null)
        {
            poolUnit = pool.New();
        }
        poolUnit.transform.position = position;
        poolUnit.transform.rotation = rotation;
        poolUnit.gameObject.SetActive(true);
        return poolUnit;
    }
    public void AddPool(string effectName, EffectPoolUnit poolUnit)
    {
        GameObjectPool<EffectPoolUnit> pool = null;
        if (m_effectPool.TryGetValue(effectName, out pool))
        {
            pool.Set(poolUnit);
        }
    }
    protected override void OnStart()
    {
        EffectTable.Instance.LoadData();

        foreach (KeyValuePair<int, EffectTable.Data> pair in EffectTable.Instance.m_effectTable)
        {
            if (!m_effectNameList.Contains(pair.Value.Prefab))//중복 걸러내기
            {
                m_effectNameList.Add(pair.Value.Prefab);
            }
        }

        for (int i = 0; i < m_effectNameList.Count; i++)
        {
            string effectName = m_effectNameList[i];
            var prefab = Resources.Load<GameObject>("Effect/" + effectName);
            m_prefabList.Add(effectName, prefab); //Load 한 prefab 기억해 놓기
            GameObjectPool<EffectPoolUnit> pool = new GameObjectPool<EffectPoolUnit>();
            m_effectPool.Add(effectName, pool);

            pool.MakePool(m_presetSize, () =>
            {
                EffectPoolUnit poolUnit = null;
                if( prefab != null )
                {
                    var obj = Instantiate(prefab);
                    poolUnit = obj.GetComponent<EffectPoolUnit>();
                    if(poolUnit == null)
                    {
                        poolUnit = obj.AddComponent<EffectPoolUnit>();
                    }
                    var effectDestroy = obj.GetComponent<EffectAutoDestroy>();
                    if (effectDestroy == null)
                    {
                        effectDestroy = obj.AddComponent<EffectAutoDestroy>();
                    }
                    poolUnit.SetEffectPool(effectName);
                    obj.SetActive(false);
                }
                return poolUnit;
            });
        }
    }

}
