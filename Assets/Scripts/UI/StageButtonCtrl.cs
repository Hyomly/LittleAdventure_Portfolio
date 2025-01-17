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
    
    public string StageInfo
    {
        get { return m_stageText.text; }
        set { m_stageText.text = value; }
    }
    void SelectStage()
    {

    }
    private void Start()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(SelectStage);
    }
}
