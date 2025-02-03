using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Coin : MonoBehaviour
{
    #region [Constants and Fields]    
    [SerializeField]
    TMP_Text m_coinText;
    int m_countCoin;

    #endregion [Constants and Fields]

    #region [Public Mathods]
 
    public void CountCoin()
    {
        gameObject.SetActive(true);
        m_countCoin++;
        ShowCoinCount();
        if (IsInvoking("ClearCoinCount"))
        {
            CancelInvoke("ClearCoinCount");
        }
        Invoke("ClearCoinCount", 1f);
    }

    #endregion [Public Mathods]

    #region [Mathods]   
   
    void ShowCoinCount()
    {
        
        m_coinText.text = (m_countCoin).ToString();
    }
    void ClearCoinCount()
    {
        m_countCoin = 0;
        gameObject.SetActive(false);
    }
    #endregion[Mathods]

}
 