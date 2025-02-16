using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Hp : MonoBehaviour
{
    #region [Constants and Fields]
    [SerializeField]
    TMP_Text m_hpText;
    [SerializeField]
    Slider m_frontHpBar;
    [SerializeField]
    Slider m_backHpBar;
    [SerializeField]

    bool m_isDamage = false;
    float m_currentHp;


    #endregion [Constants and Fields]

    #region [Public Mathods]
    public void IsDamage(bool isDamage, float currentHp)
    {
        m_isDamage = isDamage;
        m_currentHp = currentHp;
    }
    public void HpBarInit(float maxHp)
    {
        m_isDamage = false;
        m_frontHpBar.maxValue = maxHp;
        m_frontHpBar.value = maxHp;
        m_backHpBar.maxValue = maxHp;
        m_backHpBar.value = maxHp;
        m_hpText.text = (maxHp).ToString();
    }
    public void UpdateHpBar()
    {
        m_frontHpBar.value = Mathf.Lerp(m_frontHpBar.value, m_currentHp, Time.deltaTime * 10f);

        Invoke("BackHpBar_Update", 1.5f);
        m_hpText.text = (m_currentHp).ToString();
        if (m_backHpBar.value <= m_frontHpBar.value + 0.1f)
        {
            CancelInvoke();
            m_backHpBar.value = m_frontHpBar.value;
        }
    }
  
    #endregion [Public Mathods]

    #region [Mathods]
    void BackHpBar_Update()
    {
        m_backHpBar.value = Mathf.Lerp(m_backHpBar.value, m_currentHp, Time.deltaTime * 5f);
    }
   
    #endregion[Mathods]


    #region[Unity Mathods]
   
    void Update()
    {
        if (m_isDamage)
        {
            UpdateHpBar();
        }
    }
    #endregion [Unity Mathods]

}
