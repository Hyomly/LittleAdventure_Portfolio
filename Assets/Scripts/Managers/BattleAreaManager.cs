using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleAreaCtrl;
using static MonsterManager;

public class BattleAreaManager : SingletonMonobehaviour<BattleAreaManager>
{
    int m_currentIdx;
    [SerializeField]
    Transform[] m_spawnPos;
    public Transform Dummy_Spawn;

   
    public void CurrentArea()
    {
        for (int i = 0; i < StageManager.Instance.m_battleArea.Count; i++)
        {
            var currentBattleArea = StageManager.Instance.m_battleArea[i];
            if (currentBattleArea.AreaStatus == BattleAreaStatus.Using)
            {
                m_currentIdx = i;
                StartBattle();
            }
        }
    }
    public void StartBattle()
    {
        CreateBattleWall(m_currentIdx);
        Dummy_Spawn = StageManager.Instance.m_battleArea[m_currentIdx].transform.Find("Dummy_Spawn").transform;
        m_spawnPos = Dummy_Spawn.GetComponentsInChildren<Transform>();
        MonsterManager.Instance.CreateMonster(m_spawnPos);
    }
    public void EndBattle()
    {
        var area = StageManager.Instance.m_battleArea[m_currentIdx];
        area.AreaStatus = BattleAreaStatus.Clear;
        RemoveBattleWall(m_currentIdx);
    }

    void CreateBattleWall(int idx)
    {
        var obj = StageManager.Instance.m_battleArea[idx].transform.GetChild(0);
        obj.gameObject.SetActive(true);
    }
    void RemoveBattleWall(int idx)
    {
        var obj = StageManager.Instance.m_battleArea[idx].transform.GetChild(0);
        obj.gameObject.SetActive(false);
    }
   
}
