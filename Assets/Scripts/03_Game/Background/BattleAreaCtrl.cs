using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterManager;

public class BattleAreaCtrl : MonoBehaviour
{
    public enum BattleAreaStatus
    {
        None,
        Using,
        Clear
    }
    bool m_isSpwan = false;
    public bool IsSpwan => IsSpwan;
    public BattleAreaStatus AreaStatus { get; set; }
   
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!m_isSpwan)
            {
                AreaStatus = BattleAreaStatus.Using;
                BattleAreaManager.Instance.CurrentArea();
            }
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        m_isSpwan = true;
    }

    private void Start()
    {
        AreaStatus = BattleAreaStatus.None;
    }
}
