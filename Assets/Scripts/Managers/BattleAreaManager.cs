using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterManager;

public class BattleAreaManager : SingletonMonobehaviour<BattleAreaManager>
{
    int m_currentIdx;
    Transform[] m_spawnPos;
    [SerializeField]
    List<BattleAreaCtrl> m_battleArea = new List<BattleAreaCtrl>();
    [SerializeField]
    List<Transform> m_spawnArea = new List<Transform>();

    public void AddList(BattleAreaCtrl battleArea)
    {
        m_battleArea.Add(battleArea);
    }

    public void CurrentArea()
    {
        for (int i = 0; i < m_battleArea.Count; i++)
        {
            var currentBattleArea = m_battleArea[i];
            if (currentBattleArea.UsedBattleArea && !currentBattleArea.ClearBettleArea)
            {
                m_currentIdx = i;
                StartBattle();
            }
        }
    }
    public void StartBattle()
    {
        CreateBettleWall(m_currentIdx);
        m_spawnPos = m_spawnArea[m_currentIdx].GetComponentsInChildren<Transform>();
        MonsterManager.Instance.CreateMonster(m_spawnPos);
    }
    public void EndBattle()
    {
        m_battleArea[m_currentIdx].IsClear(true);
        RemoveBettleWall(m_currentIdx);
    }

    void CreateBettleWall(int idx)
    {
        var obj = m_battleArea[idx].transform.GetChild(0);
        obj.gameObject.SetActive(true);
    }
    void RemoveBettleWall(int idx)
    {
        var obj = m_battleArea[idx].transform.GetChild(0);
        obj.gameObject.SetActive(false);
    }
    private void Start()
    {
        // Monster Spawn
        
    }
    
}
