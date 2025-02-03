using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Ctrl : MonoBehaviour
{
    #region [Constants and Fields]

    [SerializeField]
    Canvas m_hud;
    [SerializeField]
    Transform m_target;

    #endregion [Constants and Fields]

    #region[Unity Mathods]
    
    protected virtual void Update()
    {
        m_hud.transform.position = m_target.position;
        m_hud.transform.rotation = Camera.main.transform.rotation;       
    }
    #endregion [Unity Mathods]
}
