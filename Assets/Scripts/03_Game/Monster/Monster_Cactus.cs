  using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Monster_Cactus : MonsterCtrl
{
    [SerializeField]
    Transform m_dummyFire;
    protected override void AnimEvent_Attack()
    {
        if (m_attackArea.IsPlayer)
        {
            var dir = m_player.transform.position - m_dummyFire.position;
            dir.y = 0;
            transform.rotation = Quaternion.FromToRotation(transform.forward, dir.normalized);
            var effect = EffectPool.Instance.Create(EffectTable.Instance.GetEffectData(6).Prefab);
            effect.transform.position = m_dummyFire.position;
            effect.transform.forward = dir.normalized;
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
        // ¸ó½ºÅÍ Á×À½
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

                    if (CheckArea(m_player.transform.position, m_attackDist))
                    {
                        SetState(AIState.Attack);
                        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Attack);
                        return;
                    }
                    SetState(AIState.Chase);
                    m_monAniCtrl.Play(MonsterAniCtrl.Motion.Monitor);
                    return;
                }
                m_idleTime += Time.deltaTime;
                break;
            case AIState.Attack:
                break;
            case AIState.Chase:               
                break;
            case AIState.Damage:
                break;
        }
    }


    protected override void Start()
    {
        base.Start();
        m_attackDist = 3.5f;

        m_dummyFire = transform.Find("Dummy_Fire").transform;
    }
}
