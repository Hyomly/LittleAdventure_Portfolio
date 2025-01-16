using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField]
    GameObject m_fan;
    [SerializeField]
    float m_speed = 3f;
    Vector3 m_dir;
    bool m_isArea = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_isArea = true;
            StartCoroutine(CoAffectWind(other.gameObject,m_dir, m_speed));
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }
    IEnumerator CoAffectWind(GameObject player, Vector3 dir, float speed)
    {
        while(m_isArea)
        {
            dir.y = 0;
            player.transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }
        
    }
    private void Start()
    {
        m_dir = m_fan.transform.forward;
    }
   
}
