using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_missions;
    int m_count = 0;


    public void ShowMission(bool show)
    {
        StartCoroutine(CoShowMission(show));
    }
    public IEnumerator CoShowMission(bool show)
    {
        if (show)
        {
            while (m_count < m_missions.Length)
            {
                for (int i = 0; i < m_missions.Length; i++)
                {
                    m_missions[i].SetActive(true);
                    m_count++;
                    yield return new WaitForSeconds(0.2f);
                }
                if(m_count >= m_missions.Length)
                {                    
                    StartCoroutine(CoShowMission(false));
                }               
            }
        }
        else
        {
            yield return new WaitForSeconds(3f);
            while (m_count >= 0)
            {
                for (int i = 0; i < m_missions.Length; i++)
                {
                    m_missions[i].SetActive(false);
                    m_count--;
                }               
            }
        }       
    }
    private void Awake()
    {
        for (int i = 0; i < m_missions.Length; i++)
        {
            m_missions[i].SetActive(false);
        }
    }
   
    
}
