using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using static MonsterManager;

public class BattleAreaManager : SingletonMonobehaviour<BattleAreaManager>
{
    int m_currentIdx;
    Transform[] m_spawnPos;
    public Transform Dummy_Spawn;

    [SerializeField]
    public List<Transform> m_spawnArea = new List<Transform>();

    public void CurrentArea()
    {
        for (int i = 0; i < StageManager.Instance.m_battleArea.Count; i++)
        {
            var currentBattleArea = StageManager.Instance.m_battleArea[i];
            if (currentBattleArea.UsedBattleArea && !currentBattleArea.ClearBettleArea)
            {
                m_currentIdx = i;
                StartBattle();
            }
        }
    }
    public void StartBattle()
    {
        CreateBattleWall(m_currentIdx);
        Dummy_Spawn = StageManager.Instance.m_battleArea[m_currentIdx].transform.FindChild("Dummy_Spawn").transform;
        m_spawnPos = Dummy_Spawn.GetComponentsInChildren<Transform>();
        MonsterManager.Instance.CreateMonster(m_spawnPos);
    }
    public void EndBattle()
    {
        StageManager.Instance.m_battleArea[m_currentIdx].IsClear(true);
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
