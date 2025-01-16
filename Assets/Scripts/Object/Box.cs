using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    int m_boxHp = 10;
    [SerializeField]
    GameObject m_coin;
    Vector3 m_pos;

    public void SetDamage( float damage )
    {
        m_boxHp -= (int)damage;
        if( m_boxHp <= 0 )
        {
            gameObject.SetActive( false );
            ItemManager.Instance.CreateCoin(transform.position);
        }
    }
   
    
}
