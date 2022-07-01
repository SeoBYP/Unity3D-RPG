using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : BaseEnemy
{
    private Dictionary<string, Transform> MagicEffectDic = new Dictionary<string, Transform>();

    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private AnimatorStateInfo stateInfo;
    private EffectPooling effectPooling;
    private float ChashDistance = 18.0f;
    private float MagicRange = 12.0f;
    private float attackRange = 3.0f;
    private float distance;
    private float prevTime = 0;
    private float targetTime = 4;

    private bool IsMagic = false;
    private bool IsMagicAttack = false;

    //public HP_Bar HP;
    public Transform EffectPos;

    Vector3 leftVec = Vector3.zero;
    Vector3 rightVec = Vector3.zero;
    Vector3 forwardVec = Vector3.zero;

    public float drawLength = 3;
    //public float angle = 40;

    public Color forwardcolor = Color.blue;
    public Color leftcolor = Color.yellow;
    public Color rightcolor = Color.red;

    public override void Init()
    {
        base.Init();
        State = EnemyState.Idle;
        rigid = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        _stat = gameObject.GetComponent<EnemyStat>();
        if (_stat != null)
        {
            _stat.SetStat(6);
        }
        animationListener = GetComponent<AnimationListener>();
        if (animationListener != null)
            animationListener.Init(_stat.Attack);
        anim = GetComponent<Animator>();
        transform.position = StartPos; 
        curHp = _stat.HP / _stat.MaxHP;
        GetEffect();
        //effectPooling = GetComponent<EffectPooling>();
        //if (effectPooling != null)
        //    effectPooling.Init();
    }

    public void CheckAttack(float Angle = 40)
    {
        forwardVec = transform.forward;

        Quaternion rot = Quaternion.AngleAxis(-Angle, Vector3.up);
        leftVec = rot * transform.forward;

        rot = Quaternion.AngleAxis(Angle, Vector3.up);
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
        //HP.hp_Bar.value = curHp;
    }

    public void GetEffect()
    {
        if(EffectPos != null)
        {
            for (int i = 0; i < EffectPos.childCount; i++)
            {
                string name = EffectPos.GetChild(i).name;
                MagicEffectDic.Add(name, EffectPos.GetChild(i));
                MagicEffectDic[name].gameObject.SetActive(false);
            }
        }
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
        if (distance < MagicRange)
            State = EnemyState.Attack;
        else if (distance > ChashDistance)
            State = EnemyState.Idle;
        //플레이어가 멀어져서 공격중에 이동처리를 해야한다면 현재 애니메이션 상태를 보고 공격
        if (stateInfo.IsTag("ATTACK"))
            return;
        if (IsMagic)
            return;
        agent.isStopped = false;
        agent.SetDestination(_player.transform.position);
        anim.SetBool("EnemyRun", true);
    }

    public override void Attack()
    {
        if (distance > MagicRange)
            State = EnemyState.MoveToTarget;
        agent.isStopped = true;
        anim.SetBool("EnemyRun", false);
        transform.LookAt(_player.transform.position);

        float elapsed = Time.time - prevTime;
        if (elapsed >= targetTime)
        {
            if(distance < attackRange)
            {
                anim.SetTrigger("EnemyAttack");
                prevTime = Time.time;
                return;
            }
            AttackIndex();
            prevTime = Time.time;
        }
    }



    public void AttackIndex()
    {
        int attackindex = Random.Range(2, 3);
        switch (attackindex)
        {
            case 1:
                anim.SetTrigger("EnemyMagic1");
                break;
            case 2:
                anim.SetTrigger("EnemyMagic2");
                break;
            case 3:
                anim.SetTrigger("EnemyMagic3");
                break;
        }
    }

    public void MagicCheck()
    {
        IsMagicAttack = true;
        float MagicLength = 10;
        Collider[] colliders = Physics.OverlapSphere(transform.position, MagicLength, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; ++i)
        {
            print("Magic");
            if (Collision(colliders[i].transform, MagicLength,20))
            {
                print("Magic Attack");
                _player.OnDegreadHp(_stat.Attack);
                IsMagicAttack = false;
            }
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
                _player.Damaged();
            }
        }
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

    public bool Collision(Transform target,float length = 3,float Angle = 40)
    {
        Vector3 targetDir = (target.position - transform.position).normalized;

        float targetCos = Vector3.Dot(forwardVec, targetDir);

        float targetAngle = Mathf.Acos(targetCos) * 180 / 3.141596f;

        float distance = Vector3.Distance(transform.position, target.position);

        if (targetAngle <= Angle && distance < length)
            return true;
        return false;
    }

    public override void OnDegreadHp(float AttackDamage)
    {
        int Damege = (int)(_stat.Defence - AttackDamage);
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

    void Update()
    {
        if (ActiveSelf == false)
            return;
        CheckAttack();
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        distance = (_player.transform.position - transform.position).magnitude;
        Run();
    }
}
