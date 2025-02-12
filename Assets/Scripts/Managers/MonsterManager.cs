using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    [SerializeField]
    PlayerCtrl m_player;    
    [SerializeField]
    GameObject[] m_monsterPrefabs;
    int m_removeCount = 0;
    int m_monsterCount = 0;

    Dictionary<int, GameObjectPool<MonsterCtrl>> m_monsPool = new Dictionary<int, GameObjectPool<MonsterCtrl>>();

    public void CreateMonster(Transform[] spawnPos)
    {
        m_monsterCount = spawnPos.Length - 1;
        for (int i = 0; i < m_monsterCount; i++) 
        {
            var spawnNum = int.Parse(spawnPos[i+1].name.Split('_')[0]);
            var mon = m_monsPool[spawnNum].Get();   
            mon.transform.position = spawnPos[i + 1].transform.position;
            mon.transform.parent.gameObject.SetActive(true);
            mon.IsDie(false);
        }
    }
    public void RemoveMonster(MonsterCtrl mon)
    {
        mon.InitMonster();
         mon.transform.parent.gameObject.SetActive(false);
        m_monsPool[mon.m_status.num].Set(mon);
        m_removeCount++;
        if(m_removeCount == m_monsterCount)
        {
            BattleAreaManager.Instance.EndBattle();
            m_removeCount = 0;
        }
    }
    
    // Start is called before the first frame update
    protected override void OnStart()
    {

        m_monsterPrefabs = Resources.LoadAll<GameObject>("Monsters");
        for(int i = 0; i< m_monsterPrefabs.Length; i++)
        {
            var monNumber = int.Parse(m_monsterPrefabs[i].name.Split('_')[0]);
            var monPrefab = m_monsterPrefabs[i];
            var pool = new GameObjectPool<MonsterCtrl>(3, () =>
            {
                var obj = Instantiate(monPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                var mon = obj.GetComponentInChildren<MonsterCtrl>();
                mon.InstancePlayer(m_player);
                mon.m_status = MonsterTable.Instance.GetStatusData(monNumber);
                return mon;
            });
            m_monsPool.Add(monNumber, pool);    
        }
        
        
    }

   
}
