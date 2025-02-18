using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Monster_Cactus : MonsterCtrl
{
    [SerializeField]
    Transform m_dummyFire;
    Vector3 m_targetPos;
    
    protected override void AnimEvent_Attack()
    {
        var dir = m_targetPos - m_dummyFire.position;
        dir.y = 0;
        m_attackArea.transform.position = m_targetPos;
        var effect = EffectPool.Instance.Create(EffectTable.Instance.GetEffectData(6).Prefab);
        effect.transform.position = m_dummyFire.position;
        effect.transform.forward = dir.normalized;        
        if (m_attackArea.IsPlayer) //attack Collider안에 player 들어옴
        {
            m_player.SetDamage(m_status.attack);
        }
    }
    public override void SetDamage(float damage)
    {
        if (!m_isDie)
        {
            // Hp Down
            m_status.hp -= Mathf.RoundToInt(damage);
            m_hudCtrl.ShowBar();
            m_hudHp.IsDamage(true, m_status.hp);
            SetState(AIState.Damage);
            m_monAniCtrl.Play(MonsterAniCtrl.Motion.Damage);
        }
        // 몬스터 죽음
        if (m_status.hp <= 0)
        {
            SetDie();
        }
    }


    protected override void BehaviorProcess()
    {
        switch (m_state)
        {
            case AIState.Idle:
                if (m_idleTime > m_idleDuration)
                {
                    m_idleTime = 0f;
                    if(CheckArea(m_player.transform.position, m_detectDist)) //추적 범위 안에 들어옴
                    {
                        SetState(AIState.Chase);
                        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Monitor);
                        return;
                    }                    
                    return;
                }
                m_idleTime += Time.deltaTime;
                break;
            case AIState.Attack:
                break;
            case AIState.Chase:
                if (CheckArea(m_player.transform.position, m_attackDist)) //공격 범위 안에 들어옴
                {
                    m_targetPos = m_player.transform.position;
                    transform.LookAt(m_player.transform.position);
                    var effect = EffectPool.Instance.Create(EffectTable.Instance.GetEffectData(11).Prefab);
                    effect.transform.position = m_player.gameObject.transform.position;
                    SetState(AIState.Attack);
                    m_monAniCtrl.Play(MonsterAniCtrl.Motion.Attack);
                    return;
                }
                break;
            case AIState.Damage:
                break;
        }
    }


    protected override void Start()
    {
        base.Start();
        m_attackDist = 3.5f;
        m_detectDist = 6f;
        var dummy = GameObject.FindGameObjectsWithTag("CactusHead");
        m_dummyFire = dummy[0].transform;
    }
}
