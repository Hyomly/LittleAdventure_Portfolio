using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class EndArea : MonoBehaviour
{
    [SerializeField]
    GameObject m_bud;
    [SerializeField]
    GameObject m_bloom;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("BloomCtrl", 2f);
        }
    }
    

    void BloomCtrl()
    {
        m_bud.SetActive(false);
        m_bloom.SetActive(true);
        GameManager.Instance.CompletedGame();
    }
    private void Start()
    {
        m_bud.SetActive(true);
        m_bloom.SetActive(false);
    }
}
