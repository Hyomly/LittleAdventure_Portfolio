using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonDontDestroy<StageManager>
{
   
    List<GameObject> m_stages = new List<GameObject>();
    List<GameObject> m_battleAreas = new List<GameObject>();

    public void SetStage(int stageIdx)
    {
        LoadScene.Instance.LoadSceneAsync(SceneState.Game);
        LoadStage();
        var stage = Instantiate(m_stages[stageIdx-1]);
        stage.transform.position = Vector3.zero;
        var battleArea = Instantiate(m_battleAreas[stageIdx-1]);
        battleArea.transform.position = Vector3.zero;
        var area = battleArea.GetComponentsInChildren<BattleAreaCtrl>();
        for(int i = 0; i < area.Length+1; i++)
        {
            BattleAreaManager.Instance.AddList(area[i+1]);
        }
    }
    void LoadStage()
    {
        var stages = Resources.LoadAll<GameObject>("prefab/Stage");
        var battleAreas = Resources.LoadAll<GameObject>("prefab/BattleArea");
        for(int i = 0;i < stages.Length;i++)
        {
            m_stages.Add(stages[i]);
            m_battleAreas.Add(battleAreas[i]);
        }
        
    }
}
