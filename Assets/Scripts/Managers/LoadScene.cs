using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum SceneState
{
    None = -1,
    Title,
    SelectStage,
    Game
}
public class LoadScene : SingletonDontDestroy<LoadScene>
{
    [SerializeField]
    Image m_loadingImage;
    [SerializeField]
    Slider m_progressBar;
    [SerializeField]
    TMP_Text m_progressLabel;
    AsyncOperation m_loadingState;
    SceneState m_state;
    [SerializeField]
    SceneState m_loadState = SceneState.None;

    public void LoadSceneAsync(SceneState state)
    {
        if (m_loadingState != null) return;
        m_loadState = state;
        m_loadingState = SceneManager.LoadSceneAsync((int)state);
        ShowLoadingPage();
    }
    public void ShowLoadingPage()
    {
        m_loadingImage.gameObject.SetActive(true);
        m_progressBar.gameObject.SetActive(true);        
    }
    public void HideLoadingPage()
    {
        m_loadingImage.gameObject.SetActive(false);
        m_progressBar.gameObject.SetActive(false);
    }
    

    private void Start()
    {
        HideLoadingPage();
    }
    private void Update()
    {
        if (m_loadingState != null)
        {
            if(m_loadingState.isDone)
            {
                m_loadingState = null;
                m_progressBar.value = 1f;
                m_progressLabel.text = "100%";
                m_state = m_loadState;
                m_loadState = SceneState.None;
                Invoke("HideLoadingPage", 1f);
            }
            else
            {
                m_progressBar.value = m_loadingState.progress;
                m_progressLabel.text = ((int)(m_loadingState.progress * 100)).ToString() + '%';
            }
        }
        
    }
}  

