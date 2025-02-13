using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGameData : SingletonDontDestroy<PlayerGameData>
{
    int m_coin;
    int m_activeStageNum = 1;

    public Dictionary<int, int> m_clearMissions = new Dictionary<int, int>();    
    public int CurActivateStage { get { return m_activeStageNum; } set { m_activeStageNum = value; } }
    public int MyCoin { get { return m_coin; } set { m_coin += value; } }


    public void ClearMissionUpdate(int stageNum, int missionNum)
    {
        m_clearMissions[stageNum] = missionNum;
    }

    void ClearMissionNum(int stageNum, int missionNum)
    {
        m_clearMissions.Add(stageNum, missionNum);
    }

    protected override void OnStart()
    {
        for (int i = 1; i < StageManager.Instance.Stages.Count + 1; i++) 
        {
            ClearMissionNum(i,0);
        }
    }
}
