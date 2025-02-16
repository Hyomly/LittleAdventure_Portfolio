using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region [Constants and Fields]
    [SerializeField]
    GameObject m_player;
    [SerializeField]
    HUD_Coin m_hudCoin;
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


    #endregion [Constants and Fields]

    #region [Public Mathods]
    public void GainCoin()
    {
        m_stageCoins++;
        m_hudCoin.CountCoin();
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
        if (m_curStage == PlayerGameData.Instance.CurActivateStage)
        {
            PlayerGameData.Instance.CurActivateStage++;
        }
        UIManager.Instance.ShowClearMission(m_mission1,m_mission2,m_mission3);
        UIManager.Instance.ShowCompletePanel();
        ClearMissionCount();
        PlayerGameData.Instance.MyCoin += m_stageCoins;
    }
    public void GameOver()
    {
        UIManager.Instance.ShowGameOver();
    }
    #endregion [Public Mathods]

    #region [Mathods]
    void ClearMissionCount()
    {
        var clearCount = 0;
        if (m_mission1 == true)
        {
            clearCount++;
        }
        if( m_mission2 == true)
        {
            clearCount++;
        }
        if(m_mission3 == true)
        {
            clearCount++;
        }
        PlayerGameData.Instance.ClearMissionUpdate(m_curStage, clearCount);
    }
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
    void SettingPlayer()
    {
        m_player.transform.position = Vector3.zero;
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

    #region [Unity Mathods]
    protected override void OnAwake()
    {
        CurStage(StageManager.Instance.m_selectStage);
        StageManager.Instance.SettingMap(m_curStage);
             
    }
    protected override void OnStart()
    {
        SettingPlayer();
        SetTime();
        UIManager.Instance.ShowMission(m_curStage);
        UIManager.Instance.ShowStageInfo(m_curStage);
        m_hudCoin.gameObject.SetActive(false);
    }
    #endregion [Unity Mathods]
}