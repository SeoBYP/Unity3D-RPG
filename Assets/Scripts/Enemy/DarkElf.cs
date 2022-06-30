using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DarkElf : BaseEnemy
{
    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private AnimatorStateInfo stateInfo;
    private ArrowPooling arrowPooling;
    float ChashDistance = 20.0f;
    float attackRange = 9.0f;
    float kickattackRange = 3.0f;
    float distance = 0;
    private float prevTime = 0;
    private float targetTime = 3;
    private bool IsKickatk = false;

   // public HP_Bar HP;

    public Transform ArrowPos;
    public Transform ArrowSkillPos;

    Vector3 leftVec = Vector3.zero;
    Vector3 rightVec = Vector3.zero;
    Vector3 forwardVec = Vector3.zero;

    public float drawLength = 3;

    public float angle = 40;

    public Color forwardcolor = Color.blue;
    public Color leftcolor = Color.yellow;
    public Color rightcolor = Color.red;


    public override void Init()
    {
        base.Init();
        State = EnemyState.Idle;
        rigid = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _stat = GetComponent<EnemyStat>();
        if (_stat != null)
            _stat.SetStat(5);
        transform.position = StartPos;
        curHp = _stat.HP / _stat.MaxHP;
        arrowPooling = GetComponent<ArrowPooling>();
        if (arrowPooling != null)
            arrowPooling.Init();
    }

    public void CheckAttack()
    {
        forwardVec = transform.forward;

        Quaternion rot = Quaternion.AngleAxis(-angle, Vector3.up);
        leftVec = rot * transform.forward;

        rot = Quaternion.AngleAxis(angle, Vector3.up);
        rightVec = rot * transform.forward;
    }

    public override void SetHP(HP_Bar hp)
    {
        if (HP == null)
        {
            HP = hp;
            HP.SetFilllAmount(curHp);
        }
    }

    public override void SetHPFillAmount()
    {
        HP.SetFilllAmount(curHp);
    }

    public override void Idle()
    {
        anim.Play("Idle");
        anim.SetBool("EnemyDie", false);
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

    public override void MoveToTarget()
    {
        if (stateInfo.IsTag("EnemyAttack"))
            return;

        agent.isStopped = false;
        //float distance = (_player.transform.position - transform.position).magnitude;
        //공격가능한 거리가 되면 공격상태로 변경한다.
        if (distance < attackRange)
        {
            State = EnemyState.Attack;
        }

        if (distance > ChashDistance)
            State = EnemyState.Idle;
        //플레이어가 멀어져서 공격중에 이동처리를 해야한다면 현재 애니메이션 상태를 보고 공격
        if (stateInfo.IsTag("ATTACK"))
            return;

        agent.SetDestination(_player.transform.position);
        anim.SetBool("EnemyRun", true);
    }

    public override void Attack()
    {
        if (stateInfo.IsTag("Pase2"))
            return;

        if (distance > attackRange)
            State = EnemyState.MoveToTarget;

        agent.isStopped = true;
        anim.SetBool("EnemyRun", false);
        transform.LookAt(_player.transform.position);

        float damege = _stat.Attack;
        float elapsed = Time.time - prevTime;
        if (elapsed >= targetTime)
        {
            if (distance < kickattackRange)
            {
                KickAttack();
                prevTime = Time.time;
                return;
            }
            AttackIndex();
            prevTime = Time.time;
        }
    }

    public void KickAttack()
    {
        anim.SetTrigger("EnemyKickAttack");
        IsKickatk = true;
    }

    public void AttackIndex()
    {
        int attackindex = Random.Range(1, 2);
        switch (attackindex)
        {
            case 1:
                anim.SetTrigger("EnemyDoAttack1");              
                break;
            case 2:
                anim.SetTrigger("EnemyDoAttack2");
                break;
        }
    }

    public void ArrowShoot()
    {
        float damage = _stat.Attack;
        Arrow arrow = ArrowPooling.Instance.Pooling();
        if (arrow != null)
        {
            arrow.SetPlayer(_player);
            arrow.Execute(transform,ArrowPos.position, damage, false);
            GameAudioManager.Instance.Play2DSound("ArrowShoot");
        }
    }

    public void Skill()
    {
        float damage = _stat.Attack;
        Arrow arrow = ArrowPooling.Instance.Pooling();
        if (arrow != null)
        {
            arrow.SetPlayer(_player);
            arrow.Execute(transform,ArrowSkillPos.position, damage, true);
        }
    }

    public void Check()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, drawLength, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (Collision(colliders[i].transform))
            {
                _player.OnDegreadHp(_stat.Attack);
                IsKickatk = false;
                _player.Damaged();
            }
        }
        IsKickatk = false;
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

    public override void OnDegreadHp(float AttackDamage)
    {
        int Damege = (int)(_stat.Defence - AttackDamage);
        if (Damege < 0)
            _stat.HP += Damege;

        DamageText damageText = DamagePooling.Instance.Pooling();
        if (damageText != null)
        {
            if (Damege > 0)
                Damege = 0;
            if (Damege < 0)
                Damege = Mathf.Abs(Damege);
            damageText.Execute(transform.position, Damege);
        }
        GameAudioManager.Instance.Play2DSound("Hited");
        _player.Isskillattack = false;
        curHp = _stat.HP / _stat.MaxHP;
        SetHPFillAmount();
        //매니저에 UIManager에 있는 BaseUI의 DegreadHp함수를 실행시킨다.
        //UIManager.Instance.Get<InGameUI>();
        if (_stat.HP <= 0)
        {
            curHp = 0;
            _stat.HP = 0;
            IsDead = true;
            State = EnemyState.Dead;

        }
    }

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

    public override void CheckDead()
    {
        if (IsGiveEXP)
        {
            int giveexp = _stat.EnemyExp;
            _player.AddExp(giveexp);
            IsGiveEXP = false;
        }
        if (HP != null)
            HP.DeAcitve();
        this.transform.gameObject.SetActive(false);
        //Destroy(gameObject);
        RemoveEnemyCount();
    }

    void Update()
    {
        if (ActiveSelf == false)
            return;
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        distance = (_player.transform.position - transform.position).magnitude;
        CheckAttack();
        Run();
    }
}
