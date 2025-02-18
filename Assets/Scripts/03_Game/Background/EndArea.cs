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
    [SerializeField]
    Transform m_fxPos;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BloomCtrl();
        }
    }
    void BloomCtrl()
    {
        var effect = EffectPool.Instance.Create(EffectTable.Instance.GetEffectData(9).Prefab);
        effect.transform.position = m_fxPos.position;
        m_bud.SetActive(false);
        m_bloom.SetActive(true);
        Invoke("CompletedGame", 1.5f);
    }
    void CompletedGame()
    {
        GameManager.Instance.CompletedGame();
    }
    private void Start()
    {
        m_bud.SetActive(true);
        m_bloom.SetActive(false);
        m_fxPos = transform.Find("Dummy_Fx");

    }
}
