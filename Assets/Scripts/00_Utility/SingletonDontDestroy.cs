using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : SingletonDontDestroy<T>
{
    static T m_instance;
    public static T Instance { get { return m_instance; } }
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void Awake() 
    {
        if(m_instance == null)
        {
            m_instance = (T)this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
    }
    void Start()
    {
        if(m_instance == this)
        {
            OnStart();
        }
    }   
}
