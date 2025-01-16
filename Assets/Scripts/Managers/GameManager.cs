using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region [Constants and Fields]

    [SerializeField]
    float m_time = 180f;
    float m_curTime;
    int m_minute;
    int m_second;
    int m_stageCoins;
    int m_curStage;
    bool m_isDamage = false;
    bool m_mission1 = false;
    bool m_mission2 = false;
    bool m_mission3 = false;


    List<BattleAreaCtrl> m_battleAreas = new List<BattleAreaCtrl>();
    #endregion [Constants and Fields]



    #region [Public Mathods]
    public void PulsCoin()
    {
        m_stageCoins++;
        UIManager.Instance.ShowCoins(m_stageCoins);
    }
    public void StartTimer()
    {
        StartCoroutine(CoTimer());
    }
    public void SetTime()
    {
        m_curTime = m_time;
        m_minute = (int)m_curTime / 60;
        m_second = (int)m_curTime % 60;
        UIManager.Instance.ShowTimer(m_minute, m_second);
    }
    public void CurStage(int stage)
    {
        m_curStage = stage;
        UIManager.Instance.ShowMission(stage);
    }
    public void IsOver(bool isOver)
    {
        m_mission1 = isOver;
    }
    public void IsDamaged()
    {
        m_isDamage= true;
    }   
    public void CompletedGame()
    {
        StopAllCoroutines();
        IsOver(true);
        CheckDamage(m_isDamage);
        CheckPlayTime(m_curTime);
        UIManager.Instance.ShowClearMission(m_mission1,m_mission2,m_mission3);
        UIManager.Instance.ShowCompletePanel();
    }
    public void GameOver()
    {
        UIManager.Instance.ShowGameOver();
    }
    #endregion [Public Mathods]

    #region [Mathods]
    void CheckDamage(bool isDamage)
    {
        if (!isDamage)
        {
            m_mission2 = true;
        }
    }
    void CheckPlayTime(float curTime)
    {
        var missionData = MissionTable.Instance.GetMissionData(m_curStage);
        if (curTime >= missionData.MissionTime)
        {
            m_mission3 = true;
        }
    }
    IEnumerator CoTimer()
    {
        while (m_curTime > 0)
        {
            m_curTime -= Time.deltaTime;
            m_minute = (int)m_curTime / 60;
            m_second = (int)m_curTime % 60;
            UIManager.Instance.ShowTimer(m_minute, m_second);
            yield return null;

            if (m_curTime <= 0)
            {
                GameOver();
                m_curTime = 0;
                yield break;
            }
        }
    }


    #endregion [Mathods]
    // Start is called before the first frame update
    protected override void OnStart()
    {
        SetTime();
        CurStage(1);
    }

}