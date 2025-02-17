using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static MonsterCtrl;

public class PlayerCtrl : MonoBehaviour
{
    #region [Constants and Fields]
    PlayerAniCtrl m_animCtrl;
    SkillCtrl m_skillCtrl;
    [SerializeField]
    HUD_Ctrl m_hudCtrl;
    HUD_Hp m_hudHp;

    AttackAreaUnitFind[] m_attackAreas;
    [SerializeField]
    GameObject m_attackAreaObj;

    [Space(10f)]
    [SerializeField, Header("[ 주인공 능력치 ]")]
    Status m_status;

    bool m_isCombo = false;
    bool m_isUseSkill1 = true;
    bool m_isUseSkill2 = true;
    bool m_isUseDesh = true;
    int hash_Move;
    float m_speed;
    Vector3 m_dir;


    #endregion [Constants and Fields]

    #region [Public  Properties]
    Motion GetMotion { get { return m_animCtrl.GetMotion; } }  //current Motion
    public bool IsAttack
    {
        get
        {
            if (GetMotion == Motion.Attack1 ||
                GetMotion == Motion.Attack2 ||
                GetMotion == Motion.Attack3 ||
                GetMotion == Motion.Skill1 ||
                GetMotion == Motion.Skill2)
                return true;
            return false;
        }
    }

    #endregion [Public Properties]

    #region [Animation Event Methods]

    void AnimEvent_AnimFinished()
    {
        m_animCtrl.Play(Motion.Idle);
    }
    void AnimEvent_Attack()
    {
        if (GetMotion != Motion.Damage)
        {
            var skill = SkillTable.Instance.GetSkillData(GetMotion);
            var curArea = m_attackAreas[skill.attackArea];
            var effectData = EffectTable.Instance.GetEffectData(skill.effectId);
            var monList = curArea.MonList;
            var objList = curArea.ObjList;
            for (int i = 0; i < monList.Count; i++)
            {
                var mon = monList[i].GetComponent<MonsterCtrl>();
                if (mon != null && mon.transform.parent.gameObject.activeSelf)
                {
                    if (mon.m_status.hp - skill.attack <= 0)
                    {
                        ClearAttackArea(mon); //몬스터 처치시 공격범위에 몬스터 지우기
                    }
                    var effect = EffectPool.Instance.Create(effectData.Prefab);
                    effect.transform.position = mon.transform.position + Vector3.up * 0.5f;
                    var dir = effect.transform.position - transform.position;
                    dir.y = 0;
                    effect.transform.rotation = Quaternion.FromToRotation(effect.transform.forward, dir.normalized);
                    mon.SetDamage(skill.attack);
                }
            }
            for (int i = 0; i < objList.Count; i++)
            {
                var obj = objList[i].GetComponent<Box>();
                if (obj != null && objList[i].gameObject.activeSelf)
                {
                    obj.SetDamage(skill.attack);
                }
            }
        }
    }

    // Combo Attack Frame
    void AnimEvent_AttackFinished()
    {
        m_isCombo = false;
        if (m_skillCtrl.CommandCount > 0)
        {
            m_skillCtrl.GetCommand();
            m_isCombo = true;
            if (m_skillCtrl.CommandCount > 0)
            {
                m_skillCtrl.ClearKeyBuffer();
                m_isCombo = false;
            }
        }
        if (m_isCombo)
        {
            m_animCtrl.Play(m_skillCtrl.GetCombo());
        }
        else
        {
            m_skillCtrl.ResetCombo();
            m_animCtrl.Play(Motion.Idle);
        }

    }
    #endregion [Animation Event Methos]

    #region [Public Methods]

    public void ClearAttackArea(MonsterCtrl mon)
    {
        for(int i = 0; i< m_attackAreas.Length; i++)
        {
            m_attackAreas[i].ClearList(mon.gameObject);
        }
    }
    public void AffectWind(Vector3 dir, float speed)
    {
        gameObject.transform.position += dir * speed * Time.deltaTime;
    }
    public void SetDamage(float damage)
    {
        if (!IsAttack)
        {
            // Hp Down
            m_status.hp -= Mathf.RoundToInt(damage);
            m_hudHp.IsDamage(true, m_status.hp);
            if (m_status.hp != m_status.hpMax)
            {
                GameManager.Instance.IsDamaged();
            }
            m_animCtrl.Play(Motion.Damage);

            // GameOver
            if (m_status.hp <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
    public void SetAttack()
    {
        if (GetMotion == Motion.Idle || GetMotion == Motion.Walk)
        {
            m_animCtrl.Play(Motion.Attack1);
        }
        else
        {
            m_skillCtrl.AddCommand(KeyCode.Space);       
        }
    }
    public void SetDesh()
    {
        if (m_isUseDesh)
        {
            UIManager.Instance.InitSlider(Motion.Desh, 3f);
            m_animCtrl.Play(Motion.Desh);
            m_isUseDesh = false;
            StartCoroutine(CoDeshCool(Motion.Desh, 3f));            
        }
               
    }
    public void SetSkill1()
    {
        if(m_isUseSkill1)
        {
            UIManager.Instance.InitSlider(Motion.Skill1, 5f);
            m_animCtrl.Play(Motion.Skill1, false);
            m_isUseSkill1 = false;
            StartCoroutine(CoSkill1Cool(Motion.Skill1, 5f));
            
        }       
    }
    public void SetSkill2()
    {
        if (m_isUseSkill2)
        {
            UIManager.Instance.InitSlider(Motion.Skill2, 5f);
            m_animCtrl.Play(Motion.Skill2, false);
            m_isUseSkill2 = false;
            StartCoroutine(CoSkill2Cool(Motion.Skill2, 5f));
            
        }
    }
   
    #endregion [Public Methods]

    #region [Methods]

    
    Vector3 GetPadAxis()
    {
        Vector3 padDir = MovePad.Instance.PadDir; 
        Vector3 dir = Vector3.zero;
        if(padDir.x < 0f)   {  dir += Vector3.left * Mathf.Abs(padDir.x);  }
        if(padDir.x > 0f)   {  dir += Vector3.right * padDir.x;  }
        if(padDir.y < 0f)   {  dir += Vector3.back * Mathf.Abs(padDir.y);  }
        if(padDir.y > 0f)   {  dir += Vector3.forward * padDir.y;  }
        return dir;  
    }
    IEnumerator CoDeshCool(Motion skill, float coolTime)
    {
        if (skill == Motion.Desh)
        {
            float curTime = coolTime;
            while (curTime > 0f)
            {
                UIManager.Instance.ShowCoolTime(skill, curTime);
                curTime -= Time.deltaTime;
                
               
                yield return null;
            }
            if (curTime <= 0f)
            {
                m_isUseDesh = true;
            }

        }
    }
    IEnumerator CoSkill1Cool(Motion skill, float coolTime)
    {
        if (skill == Motion.Skill1)
        {
            float curTime = coolTime;
            while (curTime > 0f)
            {
                UIManager.Instance.ShowCoolTime(skill, curTime);
                curTime -= Time.deltaTime;                
                yield return null;
            }
            if (curTime <= 0f)
            {
                m_isUseSkill1 = true;
            }            
        }
    }
    IEnumerator CoSkill2Cool(Motion skill, float coolTime)
    {
        if (skill == Motion.Skill2)
        {
            float curTime = coolTime;
            while (curTime > 0f)
            {
                UIManager.Instance.ShowCoolTime(skill, curTime);
                curTime -= Time.deltaTime;
                
                yield return null;
            }
            if (curTime <= 0f)
            {
                m_isUseSkill2 = true;
            }
        }

    }
  
    #endregion [Methods]

    #region [Unity Methods] 

    void Start()
    {
        m_animCtrl = GetComponent<PlayerAniCtrl>();
        m_skillCtrl = GetComponent<SkillCtrl>();       
        m_attackAreas = m_attackAreaObj.GetComponentsInChildren<AttackAreaUnitFind>();
        m_hudHp = m_hudCtrl.gameObject.GetComponent<HUD_Hp>();
        hash_Move = Animator.StringToHash("IsMove");
        m_hudHp.HpBarInit(m_status.hpMax);
    }

    void Update()
    {
        if(IsAttack)
        {
            m_speed = 0;
        }
        else
        {
            m_speed = 4;
        }
        //Attack Combo
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetAttack(); 
        }
        //Desh
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetDesh();  
        }
        //Skill 1
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SetSkill1();
        }
        //Skill 2
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetSkill2();
        }
        //Move Charactor
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        var padDir = GetPadAxis();
        if (padDir != Vector3.zero)
        {
            m_dir = padDir;           
        }
        if (m_dir != Vector3.zero && !IsAttack)
        {
            m_animCtrl.SetBool(hash_Move, true);
            transform.forward = m_dir;
        }        
        else if(!IsAttack)
        {
            m_animCtrl.SetBool(hash_Move, false);
        }
        transform.position += m_dir * m_speed * Time.deltaTime;
       
    }
    #endregion [Unity Methods]
    


}
