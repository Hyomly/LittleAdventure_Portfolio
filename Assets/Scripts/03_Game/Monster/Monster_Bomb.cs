using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bomb : MonsterCtrl
{
    protected override void AnimEvent_Attack()
    {
        if (m_attackArea.IsPlayer)
        {
            m_player.SetDamage(m_status.attack);
        }

    }
    public override void SetDamage(float damage)
    {
        if (damage > 0)
        {
            // ���� �� �и��� ȿ��
            Vector3 from = transform.position; //���� ��ġ����
            Vector3 dir = (transform.position - m_player.transform.position); //���� ����(player direction)
            dir.y = 0f;
            Vector3 to = from + dir.normalized * 0.2f;//���� �Ÿ�
            float duration = 0.2f;
            m_moveTween.Play(from, to, duration, false);

            SetState(AIState.Attack);
            m_monAniCtrl.Play(MonsterAniCtrl.Motion.Attack);
        }
    }
    protected override void AnimEvent_AttackFinished()
    {
        SetDie();
    }
    protected override void SetDie()
    {
        gameObject.transform.position = Vector3.zero;
        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Idle);
        MonsterManager.Instance.RemoveMonster(this);
         m_hudHp.HideBar();
    }
    protected override void Start()
    {
        base.Start();
    }
}
