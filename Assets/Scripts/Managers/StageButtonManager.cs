using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonManager : SingletonMonobehaviour<StageButtonManager>
{
    [SerializeField]
    Transform m_buttonZone;
    [SerializeField]
    GameObject m_stagePrefab;
    int m_stageNum = 0;
    

    List<GameObject> m_stageButtons = new List<GameObject>();

    
    public void ButtonActivate(int idx, bool isActivte)
    {
        m_stageButtons[idx].GetComponent<Button>().interactable = isActivte;
    }
    void CreateButton()
    {
        var obj = Instantiate(m_stagePrefab);
        obj.transform.SetParent(m_buttonZone, false);
        obj.GetComponent<Button>().interactable = false;
        StageButtonCtrl stageText = obj.GetComponent<StageButtonCtrl>();
        m_stageNum++;
        stageText.StageNum = m_stageNum;
        m_stageButtons.Add( obj);
    }
    
    // Start is called before the first frame update
    protected override void OnStart()
    {
       
        for(int i = 0; i< StageManager.Instance.Stages.Count; i++) 
        {
            CreateButton();
        }
        for(int i = 0; i< StageManager.Instance.CurActivateStage; i++)
        {
            ButtonActivate(i, true);
        }
        
    }

}
