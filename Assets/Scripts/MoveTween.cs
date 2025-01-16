using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTween : MonoBehaviour
{
    NavMeshAgent m_navAgent;
    [SerializeField]
    AnimationCurve m_curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    Vector3 m_from;
    [SerializeField]
    Vector3 m_to;
    [SerializeField]
    float m_duration = 1f;
    bool m_isItem = false;

    IEnumerator CoTweenProcess()
    {
        float time = 0f;
        float value = 0f;
        Vector3 pos = Vector3.zero;
        m_navAgent.ResetPath();
        while (true)
        {
            if(time > 1.0f)
            {
                transform.position = m_to;
               
                yield break;
            }
            value = m_curve.Evaluate(time);
            pos = m_from * (1f - value) + m_to * value;
            var dir = pos - transform.position;
            m_navAgent.Move(dir);
            time += Time.deltaTime / m_duration; 
            yield return null;
        }
    }
    IEnumerator CoTweenProcessCoin()
    {
        float time = 0f;
        float value = 0f;
        Vector3 pos = Vector3.zero;
        while (true)
        {
            if (time > 1.0f)
            {
                transform.position = m_to;
                yield break;
            }
            value = m_curve.Evaluate(time);
            pos = m_from * (1f - value) + m_to * value;
            var dir = pos - transform.position;
            transform.Translate(dir);
            time += Time.deltaTime / m_duration;
            yield return null;
        }
    }
    void Magnet(Vector3 playerPos)
    {

    }
    public void Play()
    {
        StopAllCoroutines();
        if(m_isItem)
        {
            StartCoroutine(CoTweenProcessCoin());    
        }
        else
        {
            StartCoroutine(CoTweenProcess());
        }
        
    }
    public void Play(Vector3 from, Vector3 to, float duration, bool isItem)
    {
        m_isItem = isItem;
        m_from = from;
        m_to = to;
        m_duration = duration;
        Play();
    }
   
    
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
    }

}
