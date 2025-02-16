using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class TextBlink : MonoBehaviour
{
    TMP_Text m_startText;
    bool m_isDecrease = true;
    [SerializeField]
    float m_rate = 0.5f;
    public IEnumerator Blink(float rate)
    {
        if (m_isDecrease)
        {
            float alphaValue = 1f;
            while (alphaValue > 0.3f)
            {
                alphaValue -= rate * Time.deltaTime;
                m_startText.alpha = alphaValue;
                if (alphaValue <= 0.3f)
                {
                    m_isDecrease = false;
                    StartCoroutine(Blink(m_rate));
                }
                yield return null;
            }
        }
        else
        {
            float alphaValue = 0.5f;
            while (alphaValue < 1f)
            {
                alphaValue += rate * Time.deltaTime;
                m_startText.alpha = alphaValue;
                if (alphaValue >= 1f)
                {
                    m_isDecrease = true;
                    StartCoroutine(Blink(m_rate));
                }
                yield return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_startText = GetComponent<TMP_Text>();
        StartCoroutine(Blink(m_rate));
    }

}
