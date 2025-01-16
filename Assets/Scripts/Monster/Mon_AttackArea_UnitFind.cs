using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_AttackArea_UnitFind : MonoBehaviour
{
    bool m_isPlayer = false;

    public bool IsPlayer => m_isPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayer = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayer = false;
        }

    }
    
}
