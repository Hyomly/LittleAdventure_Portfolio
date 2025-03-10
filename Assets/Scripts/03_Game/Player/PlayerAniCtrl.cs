using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Motion
{
    Idle,
    Walk,
    Attack1,
    Attack2,
    Attack3,
    Damage,
    Desh,
    Skill1,
    Skill2,
    Death,
    Max
}

public class PlayerAniCtrl : AnimationCtrl
{
    #region [Constants and Fields]
 
    [SerializeField]
    Motion m_curMotion;

    Dictionary<Motion, int> m_motionHashTable = new Dictionary<Motion, int>();

    #endregion [Constants and Fields]

    #region [Public  Properties]

    public Motion GetMotion { get { return m_curMotion; } }

    #endregion [Public Properties]

    #region [Methods]
    public void Play(Motion motion, bool isBlend = true)
    {
        m_curMotion = motion;
        Play(m_motionHashTable[motion], isBlend);
    }

    #endregion [Methods]

    #region [Unity Methods] 
    protected override void Start()
    {
        base.Start();
        for(int i = 0; i < (int)Motion.Max; i++)
        {
            var motion = (Motion)i;
            m_motionHashTable.Add(motion, Animator.StringToHash(motion.ToString()));
        }
    }

    #endregion [Unity Methods]
}
