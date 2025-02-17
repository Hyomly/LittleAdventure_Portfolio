using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolUnit : MonoBehaviour
{
    [SerializeField]
    float m_delay = 0.1f; //effect가 꺼지고 시간이 지나야함
    float m_inactiveTime;
    string m_effectName;

    public bool IsReady
    {
        get
        {
            if(!gameObject.activeSelf)
            {
                if(Time.time> m_inactiveTime + m_delay)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void SetEffectPool(string effectName)
    {
        m_effectName = effectName; //어느 pool에 속하는가
        transform.SetParent(EffectPool.Instance.transform); 
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    private void OnDisable()
    {
        m_inactiveTime = Time.time;
        EffectPool.Instance.AddPool(m_effectName, this);
    }
}
