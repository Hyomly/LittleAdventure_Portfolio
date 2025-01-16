using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_LookCam : MonoBehaviour
{
    [SerializeField]
    Transform m_target;
    RectTransform m_rectTrans;

    private void Start()
    {
        m_rectTrans = GetComponent<RectTransform>();
        m_rectTrans.Rotate(0, 180, 0);

    }
}
