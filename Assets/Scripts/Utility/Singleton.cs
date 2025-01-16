using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{ 
    static T m_instance;
    public static T Instance {  get { return m_instance; } }    
    static Singleton()
    {
        if( m_instance == null)
        {
            m_instance = new T();
        }
    }
}