using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    CanvasGroup m_canvasGroup;
    [SerializeField]
    public float m_rate = 0.5f;
    [SerializeField]
    MissionCtrl m_missionCtrl;
   

    public IEnumerator CoFadeInOut(bool isFadeIn, float rate)
    {
        if (isFadeIn)
        {
            float alphaValue = 0f;

            while (alphaValue < 1f)
            {
                alphaValue += rate * Time.deltaTime;

                m_canvasGroup.alpha = alphaValue;

                Mathf.Clamp(m_canvasGroup.alpha, 0f, 1f);

                if (alphaValue >= 1f)
                {
                    StartCoroutine(CoFadeInOut(false, m_rate));
                }
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            float alphaValue = 1.0f;
                
            while (alphaValue > 0f)
            {
                alphaValue -= rate * Time.deltaTime;

                m_canvasGroup.alpha = alphaValue;

                Mathf.Clamp(m_canvasGroup.alpha, 0f, 1f);

                if(alphaValue <= 0f)
                {
                    gameObject.SetActive(false);
                    m_missionCtrl.ShowMission(true);
                    GameManager.Instance.StartTimer();
                }
                yield return null;
            }

        }
    }
    void Awake()
    {
        m_canvasGroup = gameObject.GetComponent<CanvasGroup>();
        m_canvasGroup.alpha = 0;
    }
    void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine(CoFadeInOut(true, m_rate));
    }
}
