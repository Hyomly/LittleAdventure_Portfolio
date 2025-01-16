using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePad : SingletonMonobehaviour<MovePad>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region [Constants and Fields]

    [SerializeField]
    RectTransform m_stick;
    RectTransform m_rectTrans;
    
    float m_stickRange = 160;
    Vector2 m_firstTouch;
    #endregion [Constands and Fields]
    #region [Public  Properties]
    public Vector2 PadDir { get { return m_stick.anchoredPosition / m_stickRange; } }
    #endregion [Public Properties]

    #region [Mathods]
    void StickCtrl(Vector2 inputDir)
    {
        // stick 최대 거리 설정
        var clampedDir = inputDir.magnitude < m_stickRange ? inputDir : inputDir.normalized * m_stickRange;
        m_stick.anchoredPosition = clampedDir;
    }
    //터치 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_firstTouch = eventData.position;
        if(m_firstTouch == null)
        {
            m_firstTouch = Vector2.zero;
        }
        var inputDir = m_firstTouch - m_rectTrans.anchoredPosition;
        
        StickCtrl(inputDir);
    }
     
    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        var inputDir = eventData.position - m_firstTouch;
        StickCtrl(inputDir);
    }
    // 터치 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        m_stick.anchoredPosition = Vector2.zero;
    }
    #endregion [Mathods]

    #region [Unity Mathods]
    protected override void OnAwake()
    {
        m_rectTrans = GetComponent<RectTransform>();
    }
    
    #endregion [Unity Mathods]


}
