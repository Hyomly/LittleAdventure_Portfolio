using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonManager : MonoBehaviour
{
    [SerializeField]
    Transform m_buttonZone;
    [SerializeField]
    GameObject m_stagePrefab;
    int m_stageNum = 0;

    List<GameObject> m_stageButtons = new List<GameObject>();   
   

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
    public void ButtonActivate(int idx,bool isActivte)
    {
        m_stageButtons[idx].GetComponent<Button>().interactable = isActivte;
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< 4; i++) 
        {
            CreateButton();
        }
        ButtonActivate(0,true);
    }

}
