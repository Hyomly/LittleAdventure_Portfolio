using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleCtrl : MonoBehaviour, IPointerClickHandler 
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        LoadScene.Instance.LoadSceneAsync(SceneState.SelectStage);
    }

}

