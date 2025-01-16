using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            //GameManager.Instance.MakeWind(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //GameManager.Instance.MakeWind(false);
        }
    }
}
