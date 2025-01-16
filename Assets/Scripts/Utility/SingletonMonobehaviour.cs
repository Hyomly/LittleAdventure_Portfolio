using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
{
    static T m_instance;
    public static T Instance 
    { get { return m_instance; } private set { m_instance = value; } }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void Awake()
    {
        if(m_instance == null)
        {
            m_instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(m_instance == this)
        {
            OnStart();
        }
    }
}


