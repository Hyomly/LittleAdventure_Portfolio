using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



public class MonsterCtrl : MonoBehaviour
{
    #region [Constants and Fields]
    
    // Monster AiState Pattern
    public enum AIState 
    {
        Idle,
        Attack,
        Chase,
        Damage,
        Max
    }

    [SerializeField]
    PlayerCtrl m_player;
    [SerializeField]
    HUD_Ctrl m_hud;
    [SerializeField]
    MonsterAniCtrl m_monAniCtrl;
    NavMeshAgent m_navAgent;
    MoveTween m_moveTween;
    Mon_AttackArea_UnitFind m_attackArea;

    [Space(10f)]
    [SerializeField, Header("[ Ai 상태 정보 ]")]
    AIState m_state = AIState.Idle;    
    [SerializeField]
    float m_idleDuration = 3f;
    [SerializeField]
    float m_idleTime;
    [SerializeField]
    float m_attackDist = 1f;

    [Space(10f)]
    [SerializeField,Header("[ 몬스터 능력치 ]")]
    Status m_status;
    bool m_isDie = false;

    
    #endregion [Constants and Fields]

    #region [Public Properties]
 


    #endregion [Public Properties]

    #region [Animation Event Methods]

   
    void AnimEvent_DamageFinished()
    {
        SetIdle(0.1f);
    }
    void AnimEvent_AttackFinished()
    {
        SetIdle(1f);
    }
    void AnimEvent_Attack()
    {
        if (m_attackArea.IsPlayer)
        {
            m_player.SetDamage(10f);            
        }

    }

    #endregion [Animation Event Methos]

    #region [Public Methods]
    public void InstancePlayer(PlayerCtrl player)
    {
        m_player = player;
    }
    public void InitMonster()
    {
        SetState(AIState.Idle);
        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Idle);
        m_status.hp = m_status.hpMax;
        m_hud.HpBarInit(m_status.hpMax);
        
    }   
    public void IsDie(bool isDie)
    {
        m_isDie=isDie;
    }
    public void SetDamage(float damage)
    {
        if (!m_isDie)
        {
            // Hp Down
            m_status.hp -= Mathf.RoundToInt(damage);
            m_hud.IsDamage(true, m_status.hp);
            SetState(AIState.Damage);
            m_monAniCtrl.Play(MonsterAniCtrl.Motion.Damage);

            // 맞은 후 밀리는 효과
            Vector3 from = transform.position; //현재 위치에서
            Vector3 dir = (transform.position - m_player.transform.position); //맞은 방향(player direction)
            dir.y = 0f;
            Vector3 to = from + dir.normalized * 0.2f;//맞은 거리
            float duration = 0.2f;
            m_moveTween.Play(from, to, duration, false);

        }
        // 몬스터 죽음
        if (m_status.hp <= 0)
        {
            SetDie();
        }
    }

    #endregion [Public Methods]

    #region [Methods]
    void SetDie()
    {
        m_isDie = true;
        Vector3 deathPos = transform.position;
        deathPos.y += 0.3f;
        ItemManager.Instance.CreateCoin(deathPos);
        MonsterManager.Instance.RemoveMonster(this);
        m_hud.HideBar();
        
    }

    bool CheckArea(Vector3 targetPos, float dist)
    {
        var dir = targetPos - transform.position;
        if (dir.magnitude <= dist * dist)
        {
            return true;
        }
        return false;
    }
    
    void SetState(AIState state)
    {
        m_state = state;
    }
    
    void BehaviorProcess()
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
                    m_monAniCtrl.Play(MonsterAniCtrl.Motion.Walk);
                    m_navAgent.stoppingDistance = m_attackDist;
                    StartCoroutine(CoChaseToTarget(m_player.transform, 30));
                    return;
                }
                m_idleTime += Time.deltaTime;
                break;
            case AIState.Attack:
                break;
            case AIState.Chase:
                if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                {
                    SetIdle(0.5f);
                }
                break;
            case AIState.Damage:
                break;
        }
    }
    void SetIdleDuration(float duration)
    {
        m_idleTime = m_idleDuration - duration;
    }
    void SetIdle(float duration)
    {
        SetState(AIState.Idle);
        SetIdleDuration(duration);
        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Idle);
    }
   
    IEnumerator CoChaseToTarget(Transform target, int frame)
    {
        while (m_state == AIState.Chase)
        {
            m_navAgent.SetDestination(target.position);
            for (int i = 0; i < frame; i++)
            {
                yield return null;
            }
        }
    }
   
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackDist);

    }

    #endregion [Methods]

    #region [Unity Methods]    

    void Start()
    {
        m_monAniCtrl = GetComponent<MonsterAniCtrl>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_moveTween = GetComponent<MoveTween>();
        m_attackArea = GetComponentInChildren<Mon_AttackArea_UnitFind>();
        m_hud = GetComponent<HUD_Ctrl>();
        InitMonster();
    }

    void Update()
    {
        BehaviorProcess();
    }
    #endregion [Unity Methods]



}
