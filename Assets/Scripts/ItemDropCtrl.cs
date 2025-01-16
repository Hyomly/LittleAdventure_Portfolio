using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropCtrl : MonoBehaviour
{
    [SerializeField]
    AnimationCurve m_curveX = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    AnimationCurve m_curveY = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    AnimationCurve m_curveZ = AnimationCurve.Linear(0, 0, 1, 1);

    Coroutine m_coMove;
    static float m_endYPos = -6f;
    [SerializeField]
    float m_duration = 1f;


    public void SetItem(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        StartCoroutine(CoMove());
    }
    IEnumerator CoMove()
    {
        float time = 0f;
        float valueX = 0f;
        float valueY = 0f;
        Vector3 vDir = Vector3.zero;
        Vector3 hDir = Vector3.zero;
        Vector3 from = transform.position;
        Vector3 to = new Vector3(from.x, m_endYPos) + new Vector3(Random.Range(-1, 2), 0f) * 0.5f;

        while (true)
        {
            if (time > 1f)
            {
                transform.position = to;
                yield break;
            }
            valueX = m_curveY.Evaluate(time);
            valueY = m_curveY.Evaluate(time);
            vDir = from * (1f - valueY) + to * valueY;
            hDir = from * (1f - valueX) + to * valueX;
            transform.position = new Vector3(hDir.x, vDir.y);
            time += Time.deltaTime / m_duration;
            yield return null;//frame 마다 처리

        }
    }

    
}
