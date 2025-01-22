using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.CodeDom;

public class StageButtonCtrl : MonoBehaviour
{
    
    [SerializeField]
    TMP_Text m_stageText;
    [SerializeField]
    Button m_btn;

    int m_stageNum;
    public int StageNum
    {
        set { m_stageNum = value; }
    }

    void SelectStage()
    {
        StageManager.Instance.SetStage(m_stageNum);        
    }
    private void Start()
    {
        m_stageText.text = m_stageNum.ToString();
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(SelectStage);
    }
}
