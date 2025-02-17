using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour  //Effect 재생이 끝나는지를 체크
{
    [SerializeField]
    float m_lifeTime;
    float m_time;
    ParticleSystem[] m_particles;

    void OnEnable()
    {
        m_time = Time.time;
    }
    void Start()
    {
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }
    void Update()
    {
        if (m_lifeTime > 0)
        {
            if (m_time + m_lifeTime < Time.time) //
                gameObject.SetActive(false);
        }
        else
        {
            bool isPlaying = false;
            for (int i = 0; i < m_particles.Length; i++)
            {
                if (m_particles[i].isPlaying)
                {
                    isPlaying = true;
                    break;
                }
            }
            if (!isPlaying)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
