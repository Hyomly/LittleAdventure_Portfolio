using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ItemManager.Instance.RemoveCoin(this);
            GameManager.Instance.GainCoin();
        }
    }
    public void Drop(Vector3 from)
    {
        Vector3 randPos = from + (Random.insideUnitSphere * 0.5f);
        randPos.y = from.y;
        var dir = randPos - transform.position;
        var to = from + dir.normalized;
        gameObject.GetComponent<MoveTween>().Play(from, to, 0.2f,true);
       
    }

  
}
