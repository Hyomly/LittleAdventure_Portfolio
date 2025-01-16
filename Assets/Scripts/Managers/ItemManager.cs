using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonobehaviour<ItemManager>
{
    [SerializeField]
    GameObject m_coinPrefab;
    GameObjectPool<Coin> m_coinPool;

    public void CreateCoin(Vector3 curPos)
    {
        var coin = m_coinPool.Get();
        coin.transform.gameObject.SetActive(true);
        coin.Drop(curPos);
    }
    public void RemoveCoin(Coin coin)
    {
        coin.gameObject.SetActive(false);
        m_coinPool.Set(coin);

    }


    protected override void OnStart()
    {
        // Coin Object Pooling
        m_coinPool = new GameObjectPool<Coin>(5, () =>
        {
            var obj = Instantiate(m_coinPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var coin = obj.GetComponent<Coin>();            
            return coin;
        });


    }

}
