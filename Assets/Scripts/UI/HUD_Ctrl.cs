using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Ctrl : MonoBehaviour
{
    #region [Constants and Fields]
    
    [SerializeField]
    Transform m_target;

    #endregion [Constants and Fields]
    #region [Mathods]
    public void SetHUD(Transform target)
    {
        m_target = target;
    }
    public void HideBar()
    {
        gameObject.SetActive(false);
    }
    public void ShowBar()
    {
        gameObject.SetActive(true);
    }
    #endregion [Mathods]

    #region[Unity Mathods]

    protected virtual void Update()
    {
        gameObject.transform.position = m_target.position;
        gameObject.transform.rotation = Camera.main.transform.rotation;       
    }
    #endregion [Unity Mathods]
}
