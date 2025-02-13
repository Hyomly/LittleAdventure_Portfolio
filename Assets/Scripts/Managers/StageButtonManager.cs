using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonManager : SingletonMonobehaviour<StageButtonManager>
{
    [SerializeField]
    Transform m_buttonZone;
    [SerializeField]
    GameObject m_stagePrefab;
    [SerializeField]
    Toggle[] m_stars;

    int m_stageNum = 0;
    
    Dictionary<int, GameObject> m_stageButtons = new Dictionary<int, GameObject>();

    
    public void ButtonActivate(int stageNum, bool isActivte)
    {
        m_stageButtons[stageNum].GetComponentInChildren<Button>().interactable = isActivte;
    }
    public void StarIsOn(int stageNum, int starNum)
    {
        var stars = m_stageButtons[stageNum].GetComponentsInChildren<Toggle>();
        var num = 0;
        for (int i = 0; i < stars.Length; i++)
        {
            num++;
            if (num <= starNum)
            {
                stars[i].SetIsOnWithoutNotify(true);
            }
            else
            {
                return;
            }
        }
    }
    void CreateButton()
    {
        var obj = Instantiate(m_stagePrefab);
        obj.transform.SetParent(m_buttonZone, false);
        obj.GetComponentInChildren<Button>().interactable = false;
        var stars = obj.GetComponentsInChildren<Toggle>();
        for(int i = 0; i < stars.Length; i++)
        {
            stars[i].interactable = false;
        }
        StageButtonCtrl stageText = obj.GetComponentInChildren<StageButtonCtrl>();
        m_stageNum++;
        stageText.StageNum = m_stageNum;
        m_stageButtons.Add(m_stageNum, obj);
    }
   
    protected override void OnStart()
    {
        for(int i = 0; i< StageManager.Instance.Stages.Count; i++) 
        {
            CreateButton();
        }
        for (int i = 1; i < PlayerGameData.Instance.CurActivateStage + 1; i++) 
        {
            ButtonActivate(i, true);
            StarIsOn(i, PlayerGameData.Instance.m_clearMissions[i]);
        }
    }
}
