using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState
{
    None = -1,
    Title,
    Game
}
public class LoadScene : SingletonDontDestroy<LoadScene>
{
    AsyncOperation m_loadingState;
    SceneState m_state;
    [SerializeField]
    SceneState m_loadState = SceneState.None;
    public void LoadSceneAsync(SceneState state)
    {
        m_loadState = state;
        m_loadingState = SceneManager.LoadSceneAsync((int)state);
    }
}  

