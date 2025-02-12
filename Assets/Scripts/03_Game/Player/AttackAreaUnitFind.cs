using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaUnitFind : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_monList = new List<GameObject>();
    [SerializeField]
    List<GameObject> m_objList = new List<GameObject>();
    
    public List<GameObject> MonList { get { return m_monList; } }
    public List<GameObject> ObjList { get { return m_objList; } }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            m_monList.Add(other.gameObject);
        }
        if(other.CompareTag("Box"))
        {
            m_objList.Add(other.gameObject);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            m_monList.Remove(other.gameObject);
        }
        if (other.CompareTag("Box"))
        {
            m_objList.Remove(other.gameObject);
        }

    }
    public void ClearList(GameObject obj)
    {
        m_monList.Remove(obj);
        m_objList.Remove(obj);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_monList.Clear();
        m_objList.Clear();
    }  

}
