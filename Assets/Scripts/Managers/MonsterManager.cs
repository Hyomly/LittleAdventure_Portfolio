using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    [SerializeField]
    PlayerCtrl m_player;    
    [SerializeField]
    GameObject[] m_monsterPrefabs;
    [SerializeField]
    GameObject m_hudPrefab;
    [SerializeField]
    Transform m_hudTrans;

    GameObjectPool<HUD_Ctrl> m_hudPool;
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
            var hud = m_hudPool.Get();
            mon.transform.position = spawnPos[i + 1].transform.position;
            mon.transform.parent.gameObject.SetActive(true);
            mon.HUD_Pos = mon.gameObject.transform.Find("HUD_Pos").transform;
            mon.SetHUD(hud);
            hud.SetHUD(mon.HUD_Pos);         
            mon.IsDie(false);
        }
    }
    public void RemoveMonster(MonsterCtrl mon, HUD_Ctrl hud)
    {
        mon.InitMonster();       
        mon.transform.parent.gameObject.SetActive(false);
        hud.HideBar();
        m_hudPool.Set(hud);
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
        for (int i = 0; i < m_monsterPrefabs.Length; i++)
        {
            var monNumber = int.Parse(m_monsterPrefabs[i].name.Split('_')[0]);
            var monPrefab = m_monsterPrefabs[i];
            var monPool = new GameObjectPool<MonsterCtrl>(3, () =>
            {
                var obj = Instantiate(monPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                var mon = obj.GetComponentInChildren<MonsterCtrl>();
                mon.InstancePlayer(m_player);
                mon.m_status = MonsterTable.Instance.GetStatusData(monNumber);
                return mon;
            });
            m_monsPool.Add(monNumber, monPool);
        }
        m_hudPool = new GameObjectPool<HUD_Ctrl>(3, () =>
        {
            var obj = Instantiate(m_hudPrefab);
            obj.transform.SetParent(m_hudTrans,false);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
            var hud = obj.GetComponentInChildren<HUD_Ctrl>();
            return hud;
        });
        
    }

   
}
