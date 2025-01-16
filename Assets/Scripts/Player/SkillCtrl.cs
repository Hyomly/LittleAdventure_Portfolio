using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCtrl : MonoBehaviour
{
    [SerializeField]
    List<Motion> m_comboList;
    Queue<KeyCode> m_keyBuffer = new Queue<KeyCode>();

    int m_comboIndex;
    public int CommandCount {  get { return m_keyBuffer.Count;} }
    public int ComboCount {  get { return m_comboList.Count; } }

    public Motion GetCombo()
    {
        m_comboIndex++;
        if(m_comboIndex >= m_comboList.Count)
        {
            m_comboIndex = 0;
        }       
        return m_comboList[m_comboIndex];
    }

    public void AddCommand(KeyCode key)
    {
        m_keyBuffer.Enqueue(key);
        if (IsInvoking("ClearKeyBuffer"))
        {
            CancelInvoke("ClearKeyBuffer");
        }
        Invoke("ClearKeyBuffer", 0.4f);
    }

    public void ResetCombo()
    {
        m_comboIndex = 0;
    }
    public KeyCode GetCommand()
    {
        return m_keyBuffer.Dequeue();
    }
    public void ClearKeyBuffer()
    {
        m_keyBuffer.Clear();
    }
    
}
