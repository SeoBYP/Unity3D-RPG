using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Slayer : BaseEnemy
{
    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private AnimatorStateInfo stateInfo;
    float ChashDistance = 10.0f;
    float attackRange = 4.0f;
    float distance = 0;
    private float prevTime = 0;
    private float targetTime = 3;

    //public HP_Bar HP;

    Vector3 leftVec = Vector3.zero;
    Vector3 rightVec = Vector3.zero;
    Vector3 forwardVec = Vector3.zero;


    public float drawLength = 4;

    public float angle = 50;

    public Color forwardcolor = Color.blue;
    public Color leftcolor = Color.yellow;
    public Color rightcolor = Color.red;

    public override void Init()
    {
        base.Init();
        State = EnemyState.Idle;
        rigid = GetComponent<Rigidbody>();
        enemyCollider = this.GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _stat = GetComponent<EnemyStat>();
        animationListener = GetComponent<AnimationListener>();
        if(_stat != null)
        {
            _stat.SetStat(4);
        }
        transform.position = StartPos;
        curHp = _stat.HP / _stat.MaxHP;
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
       if(HP == null)
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

    //타겟으로 움직임 처리
    public override void MoveToTarget()
    {
        agent.isStopped = false;

        if (distance < attackRange)
            State = EnemyState.Attack;
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
        if (distance > attackRange)
            State = EnemyState.MoveToTarget;

        agent.isStopped = true;
        anim.SetBool("EnemyRun", false);
        transform.LookAt(_player.transform.position);

        float elapsed = Time.time - prevTime;
        if (elapsed >= targetTime)
        {
            AttackIndex();
            prevTime = Time.time;
        }
    }

    private void AttackIndex()
    {
        int attackindex = Random.Range(1, 3);
        switch (attackindex)
        {
            case 1:
                anim.SetTrigger("EnemyDoAttack1");
                break;
            case 2:
                anim.SetTrigger("EnemyDoAttack2");
                break;
            case 3:
                anim.SetTrigger("EnemyDoAttack3");
                break;
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
            }
        }
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

    public override void OnDegreadHp(float AttackDamage)
    {
        if (IsDead)
            return;

        int Damege = (int)(this._stat.Defence -  AttackDamage);
        if (Damege < 0)
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
        RemoveEnemyCount();
    }

    private void Update()
    {
        if (ActiveSelf == false)
            return;
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        distance = (_player.transform.position - transform.position).magnitude;
        CheckAttack();
        Run();
    }
}
