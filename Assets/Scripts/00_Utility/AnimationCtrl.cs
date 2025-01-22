using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtrl : MonoBehaviour
{
    Animator m_anim;

    public void SetBool(int hash, bool value)
    {
        m_anim.SetBool(hash, value);
    }
    public void Play(int animHash, bool isBlend = true)
    {
        if (isBlend)
        {
            m_anim.SetTrigger(animHash);

        }
        else
        {
            m_anim.Play(animHash, 0, 0f);
        }
    }
    protected virtual void Start()
    {
        m_anim = GetComponent<Animator>();
    }

}
