using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class StageManager : SingletonDontDestroy<StageManager>
{

    NavMeshSurface m_navMesh;
    [SerializeField]
    List<GameObject> m_stages = new List<GameObject>();
    [SerializeField]
    List<GameObject> m_battleAreas = new List<GameObject>();
    [SerializeField]
    public List<BattleAreaCtrl> m_battleArea = new List<BattleAreaCtrl>();
    
    public int m_selectStage;    
    public List<GameObject> Stages => m_stages;
    
    
    public void SetStage(int stageIdx)
    {
        m_selectStage = stageIdx;
        LoadScene.Instance.LoadSceneAsync(SceneState.Game);  
    }
    public void SettingMap(int stageIdx)
    {
        m_battleArea.Clear();
        var stage = Instantiate(m_stages[stageIdx - 1]);
        stage.transform.position = Vector3.zero;
        m_navMesh.BuildNavMesh();
        var battleArea = Instantiate(m_battleAreas[stageIdx - 1]);
        battleArea.transform.position = Vector3.zero;
        var area = battleArea.GetComponentsInChildren<BattleAreaCtrl>();
        for (int i = 0; i < area.Length; i++) 
        {
            AddList(area[i]);
        }
    }
    
    void AddList(BattleAreaCtrl battleArea)
    {
        m_battleArea.Add(battleArea);
    }

    void LoadStage()
    {
        var stages = Resources.LoadAll<GameObject>("Stages");
        var battleAreas = Resources.LoadAll<GameObject>("BattleAreas");
        for (int i = 0; i < stages.Length; i++)
        {
            m_stages.Add(stages[i]);
            m_battleAreas.Add(battleAreas[i]);
        }
    }
   
    protected override void OnAwake()
    {
        LoadStage();
        m_navMesh = GetComponentInChildren<NavMeshSurface>();       
    }
}
