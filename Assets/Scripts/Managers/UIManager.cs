using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : SingletonMonobehaviour<UIManager>
{

    #region [Constants and Fields]
    [SerializeField]
    TMP_Text m_stageInfoText;
    [SerializeField]
    TMP_Text m_timerText;
    [SerializeField]
    TMP_Text m_coinCountText;
    [SerializeField]
    TMP_Text m_stopPanelText;
    [SerializeField]
    TMP_Text[] m_startMissions;
    [SerializeField]
    TMP_Text[] m_endMissions;
    [SerializeField]
    Slider[] m_skillSliders;
    [SerializeField]
    Image[] m_stars;
    [SerializeField]
    GameObject m_completePanel;
    [SerializeField]
    GameObject m_gameOverPanel;
    [SerializeField]
    GameObject m_pauseGamePanel;

    Dictionary<Motion, Slider> m_skillTimers = new Dictionary<Motion, Slider>();

    #endregion [Constants and Fields]

    #region [Public Mathods]
    public void ShowTimer( int minute, int second )
    {
        m_timerText.text = minute.ToString("00") + ":" + second.ToString("00");
    }
    public void ShowCoins( int coinCount )
    {
        m_coinCountText.text = coinCount.ToString();
    } 
    public void ShowStageInfo(int stageNum)
    {
        var num = stageNum.ToString();
        m_stageInfoText.text = "STAGE" + num;
        m_stopPanelText.text = "STAGE" + num;
    }
    public void ShowMission(int stage)
    {
        var missionData = MissionTable.Instance.GetMissionData(stage);
        for (int i = 0; i < missionData.Mission.Length; i++)
        {
            m_startMissions[i].text = missionData.Mission[i].ToString();
            m_endMissions[i].text = missionData.Mission[i].ToString();
        }
    }

    //------------------------------------------------------------------

    public void InitSlider(Motion skill,float coolTime )
    {
        m_skillTimers[skill].maxValue = coolTime;
        m_skillTimers[skill].value = m_skillTimers[skill].maxValue;
    }
    public void ShowCoolTime(Motion skill, float curTime)
    {
        m_skillTimers[skill].value = curTime;
        if(curTime <= 0.1f)
        {
            m_skillTimers[skill].value = 0f;
        }
    }
    //----------------------------------------------------------------
    public void ShowCompletePanel()
    {
        Time.timeScale = 0f;
        m_completePanel.SetActive(true);
    }
    public void ShowClearMission(bool mission1, bool mission2, bool mission3)
    {
        if (mission1)
        {
            m_stars[0].gameObject.SetActive(true);
        }
        if (mission2)
        {
            m_stars[1].gameObject.SetActive(true);
        }
        if (mission3)
        {
            m_stars[2].gameObject.SetActive(true);
        }
    }
    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        m_gameOverPanel.SetActive(true);
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        m_pauseGamePanel.SetActive(true);
    }
    public void OnRestart()
    {
        Time.timeScale = 1f;
        LoadScene.Instance.LoadSceneAsync(SceneState.Game);
    }
    public void ReturnBack()
    {
        Time.timeScale = 1f;
        LoadScene.Instance.LoadSceneAsync(SceneState.SelectStage);
    }
    public void CountinueGame()
    {
        m_pauseGamePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion [Public Mathods]

    #region [Mathods]
    void InitStar()
    {
        for (int i = 0; i < m_stars.Length; i++) 
        {
            m_stars[i].gameObject.SetActive(false);
        }
    }
    #endregion [Mathods]

    protected override void OnStart()
    {
        m_completePanel.SetActive(false);
        m_gameOverPanel.SetActive(false);
        m_pauseGamePanel.SetActive(false);
        int skillNum = 0;
        for (int i = (int)Motion.Desh; i <= (int)Motion.Skill2; i++)
        {
            var skill = (Motion)i;
            m_skillTimers.Add(skill, m_skillSliders[skillNum]);
            skillNum++;
        }        
        InitStar();
    }
}
