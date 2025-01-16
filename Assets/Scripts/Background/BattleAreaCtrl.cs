using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterManager;

public class BattleAreaCtrl : MonoBehaviour
{
    bool m_usedBettleArea = false;
    bool m_clearBettleArea = false;

    public bool UsedBattleArea => m_usedBettleArea;
    public bool ClearBettleArea => m_clearBettleArea;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            m_usedBettleArea = true;
            BattleAreaManager.Instance.CurrentArea();
        }
    }
   
    public void IsClear(bool isClear)
    {
        m_clearBettleArea = isClear;
    }
   
    
    
    
}
