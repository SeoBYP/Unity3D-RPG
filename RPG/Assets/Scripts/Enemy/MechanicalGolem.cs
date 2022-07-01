using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MechanicalGolem : BaseEnemy
{
    public Dictionary<string, Transform> WarningEffectDic = new Dictionary<string, Transform>();
    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private AnimatorStateInfo stateInfo;
    float ChashDistance = 15.0f;
    float attackRange = 4;
    float distance;
    float prevTime = 0;
    float targetTime = 3;

    public BossHpUI BossHp;
    public Transform warningpos;

    private static bool IsBoost = false;
    private static bool Is2Pase = false;
    private bool IsSmashAttack = false;
    private bool IsSwingAttack = false;
    private static bool IsAttack = false;

    Vector3 leftVec = Vector3.zero;
    Vector3 rightVec = Vector3.zero;
    Vector3 forwardVec = Vector3.zero;

    private float drawLength = 5;
    public float angle = 50;

    public Color forwardcolor = Color.blue;
    public Color leftcolor = Color.yellow;
    public Color rightcolor = Color.red;

    public override void Init()
    {
        base.Init();
        State = EnemyState.Idle;
        rigid = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();
        //값을 얻어오는데 왜 업데이트 함수에서 null오류가 나올까?
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _stat = GetComponent<EnemyStat>();
        if (_stat != null)
        {
            _stat.SetStat(2);
        }
        animationListener = GetComponent<AnimationListener>();
        if (animationListener != null)
            animationListener.Init(_stat.Attack);
        transform.position = StartPos;
        SetWarningEffect();
        curHp = _stat.HP / _stat.MaxHP;
    }

    public void SetWarningEffect()
    {
        if(warningpos != null)
        {
            for(int i = 0; i < warningpos.childCount; i++)
            {
                string name = warningpos.GetChild(i).name;
                WarningEffectDic.Add(name, warningpos.GetChild(i));
                WarningEffectDic[name].gameObject.SetActive(false);
            }
        }
    }

    public void CheckAttack()
    {
        forwardVec = transform.forward;

        Quaternion rot = Quaternion.AngleAxis(-angle, Vector3.up);
        leftVec = rot * transform.forward;

        rot = Quaternion.AngleAxis(angle, Vector3.up);
        rightVec = rot * transform.forward;
    }

    public override void SetBossHP(BossHpUI boss)
    {
        if (BossHp != null)
        {
            BossHp.SetActive(false);
            return;
        }
        BossHp = boss;
        BossHp.SetActive(false);
    }

    public override void SetHPFillAmount()
    {
        BossHp.bosshpbar.value = curHp;
        BossHp.bosshptext.text = $"{_stat.HP} / {_stat.MaxHP}";
    }

    //기본 대기값
    public override void Idle()
    {
        if (IsAttack)
            return;
        anim.Play("Idle");
        anim.SetBool("EnemyDie", false);
        agent.speed = 0;
        SetHPFillAmount();
        //stateInfo.IsTag("IDLE")
        if (distance <= ChashDistance)
            State = EnemyState.MoveToTarget;

        else if (transform.position != StartPos)
        {
            anim.SetBool("EnemyRun", true);
            agent.SetDestination(StartPos);
            float startdistance = (StartPos - transform.position).magnitude;
            if (startdistance < 0.1f)
            {
                anim.SetBool("EnemyRun", false);
            }
        }
    }
    //타겟으로 움직임 처리
    public override void MoveToTarget()
    {
        if (IsAttack)
            return;
        //플레이어가 멀어져서 공격중에 이동처리를 해야한다면 현재 애니메이션 상태를 보고 공격
        if (stateInfo.IsTag("ATTACK") || stateInfo.IsTag("Pase2") || stateInfo.IsTag("2PaseAttack"))
            return;
        agent.isStopped = false;
        agent.speed = _stat.Speed;
        //float distance = (_player.transform.position - transform.position).magnitude;
        //공격가능한 거리가 되면 공격상태로 변경한다.
        if (distance < attackRange)
        {
            if (!Is2Pase)
                State = EnemyState.Attack;
            else
                State = EnemyState.Pase2;
        }
            
        if (distance > ChashDistance)
            State = EnemyState.Idle;

        agent.SetDestination(_player.transform.position);
        anim.SetBool("EnemyRun", true);
    }

    //공격처리
    public override void Attack()
    {
        if (Is2Pase)
            return;
        if (stateInfo.IsTag("Pase2") || stateInfo.IsTag("ATTACK"))
            return;

        if (_stat.HP <= 100)
        {
            if (!Is2Pase)
            {
                StartCoroutine(SetPase2());
            }
        }
        if (_stat.HP <= 150)
        {
            if (!IsBoost)
            {
                StartBoost();
            }
        }

        if (distance > attackRange)
            State = EnemyState.MoveToTarget;
        agent.isStopped = true;
        agent.speed = 0;
        anim.SetBool("EnemyRun", false);

        float elapsed = Time.time - prevTime;
        if (elapsed >= targetTime)
        {
            AttackIndex();
            prevTime = Time.time;
        }
    }

    public void StartBoost()
    {
        StartCoroutine(Boost());
    }

    IEnumerator Boost()
    {
        IsBoost = true;
        anim.SetTrigger("Boost");
        float prevDefence = _stat.Defence;
        float prevAttack = _stat.Attack;
        _stat.Defence *= 1.5f;
        _stat.Attack *= 1.5f;
        yield return new WaitForSeconds(10.0f);
        _stat.Defence = prevDefence;
        _stat.Attack = prevAttack;
        yield return new WaitForSeconds(20.0f);
        IsBoost = false;
    }

    public void AttackIndex()
    {
        transform.LookAt(_player.transform.position);
        IsAttack = true;
        int attackindex = Random.Range(1, 3);
        switch (attackindex)
        {
            case 1:
                angle = 40;
                 ActiveWarningEffect("GroundCrackWarning", "EnemyDoAttack1");
                //anim.SetTrigger("EnemyDoAttack1");
                IsSmashAttack = true;
                break;
            case 2:
                angle = 80;
                ActiveWarningEffect("SwingWarning", "EnemyDoAttack2");
                IsSwingAttack = true;
                break;
            case 3:
                angle = 80;
                ActiveWarningEffect("SwingWarning", "EnemyDoAttack3");
                IsSwingAttack = true;
                break;
        }
    }

    public override void Pase2Attack()
    {
        if (stateInfo.IsTag("Pase2") || stateInfo.IsTag("ATTACK"))
            return;
        if (distance > attackRange)
            State = EnemyState.MoveToTarget;
        agent.isStopped = true;
        agent.speed = 0;
        anim.SetBool("EnemyRun", false);
        float elapsed = Time.time - prevTime;
        if (elapsed >= targetTime)
        {
            Pase2AttackIndex();
            prevTime = Time.time;
        }
    }

    public void Pase2AttackIndex()
    {
        int attackindex = Random.Range(1, 3);
        IsAttack = true;
        transform.LookAt(_player.transform.position);
        switch (attackindex)
        {
            case 1:
                angle = 160;
                ActiveWarningEffect("RollAttackWarning", "2PaseAttack1");
                //anim.SetTrigger("2PaseAttack1");
                break;
            case 2:
                angle = 50;
                IsSmashAttack = true;
                ActiveWarningEffect("GroundCrackWarning", "2PaseAttack2");
                //anim.SetTrigger("2PaseAttack2");
                break;
            case 3:
                angle = 80;
                ActiveWarningEffect("SwingWarning", "2PaseAttack3");
                IsSwingAttack = true;
                //anim.SetTrigger("2PaseAttack4");
                break;
        }
    }

    public void ActiveWarningEffect(string name,string attackname)
    {
        if (WarningEffectDic.ContainsKey(name))
        {
            Transform trans = WarningEffectDic[name];
            StartCoroutine(DeActiveWaringEffect(trans,attackname));
        }
        
    }

    IEnumerator DeActiveWaringEffect(Transform trans,string attackname)
    {
        trans.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        trans.gameObject.SetActive(false);
        if (attackname == "2PaseAttack3")
        {
            IsSwingAttack = true;
        }
        anim.SetTrigger(attackname);
        IsAttack = false;
        //anim.SetTrigger("DoIdle");
        yield return null;
    }

    //공격했을 때 맞았는지 체크
    public void Check()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, drawLength, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (Collision(colliders[i].transform))
            {
                _player.OnDegreadHp(_stat.Attack);
                if(IsSmashAttack)
                {
                    IsSmashAttack = false;
                    _player.SmashHited();
                }
                if (IsSwingAttack)
                {
                    IsSwingAttack = false;
                    _player.Damaged();
                }
            }
        }
        IsSmashAttack = false;
        IsSwingAttack = false;
    }

    public bool Collision(Transform target)
    {
        Vector3 targetDir = (target.position - transform.position).normalized;

        float targetCos = Vector3.Dot(forwardVec, targetDir);

        float targetAngle = Mathf.Acos(targetCos) * 180 / 3.141596f;

        float distance = Vector3.Distance(transform.position, target.position);

        if (targetAngle <= angle && distance < drawLength)
            return true;
        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = forwardcolor;
        Gizmos.DrawLine(transform.position, transform.position + forwardVec * drawLength);
        Gizmos.color = leftcolor;
        Gizmos.DrawLine(transform.position, transform.position + leftVec * drawLength);
        Gizmos.color = rightcolor;
        Gizmos.DrawLine(transform.position, transform.position + rightVec * drawLength);
    }

    public void ActiveBoost()
    {
        anim.SetTrigger("Boost");
    }

    public void DeactiveMotion()
    {
        anim.applyRootMotion = false;
    }

    //체력감소
    public override void OnDegreadHp(float AttackDamage)
    {
        if (stateInfo.IsTag("Pase2"))
            return;

        int Damege = (int)(_stat.Defence - AttackDamage);
        if(Damege < 0)
            _stat.HP += Damege;

        DamageText damageText = DamagePooling.Instance.Pooling();
        if (damageText != null)
        {
            if (Damege > 0)
                Damege = 0;
            if (Damege < 0)
                Damege = Mathf.Abs(Damege);
            damageText.Execute(transform.position, Damege, true);
        }

        _player.Isskillattack = false;
        curHp = _stat.HP / _stat.MaxHP;
        SetHPFillAmount();

        
        if (_stat.HP <= 0)
        {
            curHp = 0;
            _stat.HP = 0;
            IsDead = true;
            State = EnemyState.Dead;
        }
    }

    IEnumerator SetPase2()
    {
        Is2Pase = true;
        GameAudioManager.Instance.PlayLoopInTime("SetPase2", 1, true, 11.0f);
        anim.SetTrigger("2Pase");
        yield return new WaitForSeconds(11.0f);
        anim.SetTrigger("2Pase");
        _stat.BossPase2Stat();
        yield return new WaitForSeconds(1.0f);
        State = EnemyState.Idle;
    }

    
    //죽었을 때 죽는 애니메이션을 실행
    public override void OnDead()
    {
        if (IsDead)
        {
            anim.SetBool("EnemyDie", true);
            IsGiveEXP = true;
        }
        else
            return;
    }
    //죽었을 때 경험치를 주는 처리
    public override void CheckDead()
    {
        if (IsGiveEXP)
        {
            int giveexp = _stat.EnemyExp;
            _player.AddExp(giveexp);
            IsGiveEXP = false;
        }
        if (BossHp != null)
            BossHp.SetActive(false);
        transform.gameObject.SetActive(false);
        RemoveEnemyCount();
    }

    private void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        distance = (_player.transform.position - transform.position).magnitude;
        CheckAttack();
        Run();
    }
}
