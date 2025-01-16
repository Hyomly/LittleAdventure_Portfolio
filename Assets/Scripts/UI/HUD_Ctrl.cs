using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Ctrl : MonoBehaviour
{
    [SerializeField]
    Canvas m_hud;
    [SerializeField]
    TMP_Text m_hpText;
    [SerializeField]
    Transform m_target;
    [SerializeField]
    Slider m_frontHpBar;
    [SerializeField]
    Slider m_backHpBar;

    bool m_isDamage = false;
    float m_currentHp;
    public void IsDamage(bool isDamage, float currentHp)
    {
        m_isDamage = isDamage;
        m_currentHp = currentHp;
        
    }
    public void HpBarInit(float maxHp)
    {
        m_isDamage = false ;
        m_frontHpBar.maxValue = maxHp;        
        m_frontHpBar.value = maxHp;
        m_backHpBar.maxValue = maxHp;
        m_backHpBar.value = maxHp;
        m_hpText.text = (maxHp).ToString();
    }
    public void UpdateBar()
    {
        if(gameObject.CompareTag("Monster"))
        {
            ShowBar();
        }
        m_frontHpBar.value = Mathf.Lerp(m_frontHpBar.value, m_currentHp , Time.deltaTime * 10f );
       
        Invoke("BackHpBar_Update", 1.5f);
        m_hpText.text = (m_currentHp).ToString();
        if( m_backHpBar.value <= m_frontHpBar.value + 0.1f)
        {
            CancelInvoke();
            m_backHpBar.value = m_frontHpBar.value;
        }
    }
    
    void BackHpBar_Update()
    {
        m_backHpBar.value = Mathf.Lerp(m_backHpBar.value, m_currentHp, Time.deltaTime * 5f);

    }
    void ShowBar()
    {
        m_hud.gameObject.SetActive(true);
    }
    public void HideBar()
    {
        m_hud.gameObject.SetActive(false);
    }
    
    
   
    private void Awake()
    {
        if(gameObject.CompareTag("Player"))
        {
            ShowBar();
        }
        else
        {
            HideBar();
        }
   
    }
    private void Update()
    {
        m_hud.transform.position = m_target.position;
        m_hud.transform.rotation = Camera.main.transform.rotation;
        if(m_isDamage)
        {
            UpdateBar();
        }
    }


}
