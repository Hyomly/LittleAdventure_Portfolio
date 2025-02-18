using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    protected PlayerCtrl m_player;    
    [SerializeField]
    protected MonsterAniCtrl m_monAniCtrl;
    NavMeshAgent m_navAgent;
    protected MoveTween m_moveTween;
    protected Mon_AttackArea_UnitFind m_attackArea;
    protected HUD_Ctrl m_hudCtrl;
    protected HUD_Hp m_hudHp;
    public Transform HUD_Pos;
    protected bool m_isDie = false;

    [Space(10f)]
    [SerializeField, Header("[ Ai 상태 정보 ]")]
    protected AIState m_state = AIState.Idle;    
    [SerializeField]
    protected float m_idleDuration = 3f;
    [SerializeField]
    protected float m_idleTime;
    [SerializeField]
    protected float m_attackDist = 1f;
    [SerializeField]
    protected float m_detectDist = 1f;


    [Space(10f)]
    [SerializeField,Header("[ 몬스터 능력치 ]")]
    public Status m_status;
    

    
    #endregion [Constants and Fields]


    #region [Animation Event Methods]

   
    void AnimEvent_DamageFinished()
    {
        SetIdle(0.1f);
    }
    protected virtual void AnimEvent_AttackFinished()
    {
        SetIdle(1f);
    }
    protected virtual void AnimEvent_Attack()
    {
        if (m_attackArea.IsPlayer)
        {
            m_player.SetDamage(m_status.attack);            
        }
    }

    #endregion [Animation Event Methos]

    #region [Public Methods]
    public void InstancePlayer(PlayerCtrl player)
    {
        m_player = player;
    }
    public void SetHUD(HUD_Ctrl hud)
    {
        m_hudCtrl = hud;
        m_hudHp = m_hudCtrl.gameObject.GetComponent<HUD_Hp>();
        m_hudHp.HpBarInit(m_status.hpMax);
    }
    public void InitMonster()
    {
      
        SetState(AIState.Idle);
        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Idle);
        m_status.hp = m_status.hpMax;        
        
    }   
    public void IsDie(bool isDie)
    {
        m_isDie=isDie;
    }
    public virtual void SetDamage(float damage)
    {
        if (!m_isDie)
        {
            // Hp Down
            m_status.hp -= Mathf.RoundToInt(damage);
            m_hudCtrl.ShowBar();
            m_hudHp.IsDamage(true, m_status.hp);
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
    protected virtual void SetDie()
    {
        IsDie(true);
        Vector3 deathPos = transform.position;
        deathPos.y += 0.3f;
        ItemManager.Instance.CreateCoin(deathPos);
        gameObject.transform.position = Vector3.zero;
        MonsterManager.Instance.RemoveMonster(this, m_hudCtrl);
    }

    protected bool CheckArea(Vector3 targetPos, float dist)   //AttackArea안에 들어 왔나?
    {
        var dir = targetPos - transform.position;
        var dis = dist * dist;
        if (dir.sqrMagnitude <= dist * dist)
        {
            return true;
        }
        return false;
    }
    
    protected void SetState(AIState state)
    {
        m_state = state;
    }
    
    protected virtual void BehaviorProcess()
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
                    StartCoroutine(CoChaseToTarget(m_player.transform, 20));
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
    protected void SetIdle(float duration)
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
        Gizmos.DrawWireSphere(transform.position, m_detectDist);
    }

    #endregion [Methods]

    #region [Unity Methods]    
   
    protected virtual void Start()
    {
        m_monAniCtrl = GetComponent<MonsterAniCtrl>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_moveTween = GetComponent<MoveTween>();
        m_attackArea = GetComponentInChildren<Mon_AttackArea_UnitFind>();
       
        InitMonster();
    }

    protected virtual void Update()
    {
        BehaviorProcess();
    }
    #endregion [Unity Methods]
}
