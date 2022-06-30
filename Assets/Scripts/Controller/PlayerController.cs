using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public List<InGameSkillSlot> HasSkillList = new List<InGameSkillSlot>();
    private Rigidbody _rigidbody;
    private AnimationListener listener;
    private Collider _collider;
    private Animator ani;
    private CameraController _cameraController;
    private PlayerMashRender PlayerMash;
    private WeaponController weaponController;
    private PlayerStat _stat;
    public Transform followTransform;
    public Transform LevelUpEffect;

    [SerializeField] private float _rotationSharpness = 10f;
    [SerializeField] private float _moveSharpness = 10f;

    private Vector3 moveVec = Vector3.zero;
    private Quaternion _targetRotation;
    private float _targetSpeed;

    private float PrevTime = 0;
    private float NextAniTime = 0.5f;
    private float MoveIdleAniTime = 0.8f;
    private int aniComboCount = 0;
    private int skilltableID;
    private bool IsDead = false;
    private bool IsStop = false;
    private bool IsWall = false;

    private float _newSpeed;
    private Vector3 _newVelocity;
    private Quaternion _newRotation;
    private AnimatorStateInfo stateInfo;
    public bool Isskillattack = false;

    Vector3 leftVec = Vector3.zero;
    Vector3 rightVec = Vector3.zero;
    Vector3 forwardVec = Vector3.zero;

    public float drawLength = 3;

    public float angle = 40;

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _stat = GetComponent<PlayerStat>();
        if (_stat != null)
            _stat.SetStat(1);
        ani = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        followTransform = GameObject.FindGameObjectWithTag("FollowTransform").transform;
        _cameraController = FindObjectOfType<CameraController>();
        PlayerMash = GetComponentInChildren<PlayerMashRender>();
        if (PlayerMash != null)
        {
            PlayerMash.GetRenderer();
        }
        weaponController = GetComponent<WeaponController>();
        if (weaponController != null)
            weaponController.Init();
        if (LevelUpEffect != null)
            DeactiveLevelUpEffect();
        PlayerReset();
    }

    public void PlayerReset()
    {
        if (IsDead)
        {
            IsDead = false;
            ani.SetTrigger("PlayerIdle");
            _stat.HP = _stat.MaxHP;
            InGameUI gameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
            if (gameUI != null)
            {
                float value = _stat.HP / _stat.MaxHP;
                gameUI.SetHPFillAmount(value);
            }
        }
    }

    void Update()
    {
        if (IsDead == true)
            return;
        if (MenuUI.IsMenuOpen || StageClear.IsStageClear)
            return;
        stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        Move();
        Attack();
        Jumping();
        CheckAttack();
        PlyerSkillAttack();
        IgnoreMove();
    }

    private void FixedUpdate()
    {
        IgnoreMove();
    }

    public void SetWeapon(string name)
    {
        weaponController.ActiveWeapon(name);
    }

    public void CheckAttack(float Angle = 40)
    {
        forwardVec = transform.forward;

        Quaternion rot = Quaternion.AngleAxis(-Angle, Vector3.up);
        leftVec = rot * transform.forward;

        rot = Quaternion.AngleAxis(Angle, Vector3.up);
        rightVec = rot * transform.forward;
    }

    void Move()
    {
        if (stateInfo.IsTag("PlayerAttack1Combo") || stateInfo.IsTag("SkillAttack") || stateInfo.IsTag("PlayerHited"))
            return;

        float _XVectormove = InputManager.MoveX;
        float _YVectormove = InputManager.MoveY;

        Vector3 _moveInputVector = new Vector3(_XVectormove, 0, _YVectormove);
        Vector3 _cameraPlanarDirection = _cameraController.CameraPlanerDirection;
        Quaternion _cameraPlanarRotation = Quaternion.LookRotation(_cameraPlanarDirection);
        Debug.DrawLine(transform.position, transform.position + _moveInputVector, Color.green);

        _moveInputVector = _cameraPlanarRotation * _moveInputVector;
        Debug.DrawLine(transform.position, transform.position + _moveInputVector, Color.red);

        if (InputManager.Roll)
        {
            if (stateInfo.IsTag("PlayerRoll"))
                return;
            Roll();
        }

        _targetSpeed = _moveInputVector != Vector3.zero ? _stat.Speed : 0;
        _newSpeed = Mathf.Lerp(_newSpeed, _targetSpeed, Time.deltaTime * _moveSharpness);
        _newVelocity = _moveInputVector * _newSpeed;

        if (!IsWall)
        {
            transform.Translate(_newVelocity * Time.deltaTime, Space.World);
        }
        if (_targetSpeed != 0)
        {
            _targetRotation = Quaternion.LookRotation(_moveInputVector);
            _newRotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _rotationSharpness);
            transform.rotation = _newRotation;
        }

        ani.SetBool("IsRun", _targetSpeed != 0);
    }

    private void Roll()
    {
        if (stateInfo.IsTag("PlayerJump") || stateInfo.IsTag("PlayerAttack") || stateInfo.IsTag("PlayerHited"))
        {
            return;
        }

        ani.SetTrigger("DoRoll");
    }

    void Jumping()
    {
        if (stateInfo.IsTag("PlayerRoll") || stateInfo.IsTag("PlayerAttack") || stateInfo.IsTag("PlayerJump") || stateInfo.IsTag("PlayerHited"))
        {
            return;
        }
        if (InputManager.Jump)
        {
            _rigidbody.AddForce(Vector3.up * 7, ForceMode.Impulse);
            GameAudioManager.Instance.Play2DSound("Jump");
            ani.SetTrigger("DoJump");
        }
    }
    void Attack()
    {
        if (Item_Slot.dragging || WeaponShopUI.IsPopupOpen)
            return;
        if (stateInfo.IsTag("PlayerJump") || stateInfo.IsTag("PlayerRoll") || stateInfo.IsTag("PlayerHited"))
        {
            return;
        }
        if (stateInfo.IsTag("PlayerAttack1Combo"))
        {
            if (stateInfo.normalizedTime > MoveIdleAniTime)
            {
                if (ani.IsInTransition(0))
                {
                    return;
                }
                aniComboCount = 0;
                ani.SetInteger("Combo", aniComboCount);
                ani.SetTrigger("PlayerIdle");
            }
        }
        if (InputManager.Attack)
        {
            if (stateInfo.IsTag("Idle"))
            {
                aniComboCount = 1;
                ani.SetInteger("Combo", aniComboCount);
            }
            else
            {
                if (stateInfo.normalizedTime > NextAniTime)
                {
                    if (ani.IsInTransition(0))
                        return;
                    ++aniComboCount;
                    ani.SetInteger("Combo", aniComboCount);
                }
            }
        }
    }

    public void PlyerSkillAttack()
    {
        if (Item_Slot.dragging || WeaponShopUI.IsPopupOpen)
            return;
        if (stateInfo.IsTag("PlayerJump") || stateInfo.IsTag("PlayerRoll") || stateInfo.IsTag("PlayerAttack1Combo"))
        {
            return;
        }
        if (InputManager.SKill1)
            SkillTriger(0);
        if (InputManager.SKill2)
            SkillTriger(1);
        if (InputManager.SKill3)
            SkillTriger(2);
        if (InputManager.SKill4)
            SkillTriger(3);
        if (InputManager.SKill5)
            SkillTriger(4);
    }

    public void SkillTriger(int skillslottableid)
    {
        bool haveskill = HasSkillList[skillslottableid].IsHaveSkill;
        bool skillupdate = HasSkillList[skillslottableid].update;
        if (!haveskill || skillupdate)
            return;
        skilltableID = HasSkillList[skillslottableid].SkillId;

        float needskillmp = PlayerSkill.PlayerSkillStatDic[skilltableID].NeedMP;
        if (_stat.MP < needskillmp)
        {
            return;
        }
        string skillname = HasSkillList[skillslottableid].Skillname;
        HasSkillList[skillslottableid].Execute();
        ani.SetTrigger(skillname);
        OnDegreadMp(skilltableID, needskillmp);
        MoveDash(skillname);
        Isskillattack = true;
    }

    public void MoveDash(string name)
    {
        if (name != "IIseom")
            return;
        ani.applyRootMotion = true;
    }

    public void DeactiveMotion()
    {
        ani.applyRootMotion = false;
    }

    public float SetSkillAngle(int skilltableid)
    {
        string skillname = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillName;
        switch (skillname)
        {
            case "Prick":
                return 30;
            case "SwordStrike":
                return 50;
            case "IIseom":
                return 50;
            case "FlowerVortex":
                return 30;
        }
        return 40;
    }

    public float SetSkillDistance(int skilltableid)
    {
        string skillname = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillName;
        switch (skillname)
        {
            case "Prick":
                return 6;
            case "SwordStrike":
                return 4;
            case "IIseom":
                return 4;
            case "FlowerVortex":
                return 6;
        }
        return 3;
    }

    public void StartAnger()
    {
        StartCoroutine(Anger());
    }

    IEnumerator Anger()
    {
        float currentattack = _stat.Attack;
        float currentdefence = _stat.Defence;
        float currentspeed = _stat.Speed;
        float skilltime = PlayerSkill.PlayerSkillStatDic[2].SkillTime;
        AngerStat();
        yield return new WaitForSeconds(skilltime);
        _stat.Attack = currentattack;
        _stat.Defence = currentdefence;
        _stat.Speed = currentspeed;
    }

    public void AngerStat()
    {
        float attackper = 1 + (PlayerSkill.PlayerSkillStatDic[2].AttackPercent / 100);
        float defenceper = 1 + (PlayerSkill.PlayerSkillStatDic[2].DefencePercent / 100);
        float speedper = 1 + (PlayerSkill.PlayerSkillStatDic[2].SpeedPercent / 100);

        _stat.Attack *= attackper;
        _stat.Defence *= defenceper;
        _stat.Speed *= speedper;
    }

    public void OnDegreadMp(int skilltable, float skillmp)
    {
        InGameUI ingameui = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        if (ingameui != null)
        {
            _stat.MP -= skillmp;
            float newMp = _stat.MP / _stat.MaxMP;
            ingameui.SetFillMPAmount(newMp);
        }
    }

    public float SkillDamage(int skilltableid)
    {
        float attackpercent = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillAttackPercent;
        float skillattack = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillAttack;
        float skillcritical = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillCritical;

        float newcritical = _stat.Ciritical + skillcritical;
        float newattackper = (attackpercent / 100.0f) * _stat.Attack;
        float newdamage = _stat.Attack + newattackper + skillattack;

        float ran = Random.Range(1, 100);
        if (ran < newcritical)
        {
            float cri = newdamage * (newcritical / 100.0f);
            float criattackdamage = newdamage + cri;
            return criattackdamage;
        }
        return newdamage;
    }

    public void AddHp(int itemhp)
    {
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        PlayerState state = UIManager.Instance.Get<PlayerState>(UIList.PlayerState);
        if (inGameUI != null)
        {
            _stat.HP += itemhp;
            if (_stat.HP > _stat.MaxHP)
                _stat.HP = _stat.MaxHP;
            float newHP = _stat.HP / _stat.MaxHP;
            inGameUI.SetHPFillAmount(newHP);
            state.SetInStateHPBar(newHP);
        }
    }

    public void AddMp(int itemmp)
    {
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        PlayerState state = UIManager.Instance.Get<PlayerState>(UIList.PlayerState);
        if (inGameUI != null)
        {
            _stat.MP += itemmp;
            if (_stat.MP > _stat.MaxMP)
                _stat.MP = _stat.MaxMP;
            float newMP = _stat.MP / _stat.MaxMP;
            inGameUI.SetFillMPAmount(newMP);
            state.SetInStateMPBar(newMP);
        }
    }

    public void AddExp(int exp)
    {
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        PlayerState state = UIManager.Instance.Get<PlayerState>(UIList.PlayerState);
        PlayerSkillPopupUI skillPopupUI = UIManager.Instance.Get<PlayerSkillPopupUI>(UIList.PlayerSkillPopupUI);
        if (inGameUI != null)
        {
            _stat.Exp += exp;
            int Prevlevel = _stat.Level;
            while (_stat.Exp >= _stat.MaxExp)
            {
                _stat.PlayerLevelUP();
                state.SetStatText();
                skillPopupUI.ReSetSkillSlot();
                SetFillPlayer();
            }
            if (Prevlevel < _stat.Level)
            {
                ShowLevelUpEffect();
            }
            float expvalue = _stat.Exp / _stat.MaxExp;
            int curLevel = _stat.Level;
            inGameUI.SetPlayerLevelText();
            inGameUI.SetFillEXPAmount(expvalue);
            state.SetInStateEXPBar(expvalue);
        }
    }

    public void ShowLevelUpEffect()
    {
        if (LevelUpEffect != null)
        {
            LevelUpEffect.gameObject.SetActive(true);
            Invoke("DeactiveLevelUpEffect", 2.0f);
        }
    }

    public void DeactiveLevelUpEffect()
    {
        if (LevelUpEffect != null)
        {
            LevelUpEffect.gameObject.SetActive(false);
        }
    }

    public void SetFillPlayer()
    {
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        float newhp = _stat.HP / _stat.MaxHP;
        float newmp = _stat.MP / _stat.MaxMP;

        inGameUI.SetHPFillAmount(newhp);
        inGameUI.SetFillMPAmount(newmp);
    }

    public void OnDegreadHp(float EnemyDamage)
    {
        if (IsDead == true)
            return;
        if (stateInfo.IsTag("PlayerHited") || stateInfo.IsTag("PlayerRoll") || IsDead == true)
            return;
        PlayerMash.OnHited();
        float Damage = (_stat.Defence - EnemyDamage);
        if (Damage < 0)
            _stat.HP += Damage;
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        if (inGameUI != null)
            inGameUI.SetHPFillAmount(_stat.HP / _stat.MaxHP);

        if (_stat.HP <= 0)
        {
            _stat.HP = 0;
            ani.applyRootMotion = true;
            GameAudioManager.Instance.Play2DSound("Death");
            ani.SetTrigger("Die");
            inGameUI.DefeateStage();
            IsDead = true;
        }
    }

    public void SmashHited()
    {
        if (stateInfo.IsTag("PlayerRoll") || IsDead == true)
            return;
        ani.SetTrigger("SmashHited");
    }

    public void Damaged()
    {
        if (stateInfo.IsTag("PlayerRoll") || IsDead == true)
            return;
        _rigidbody.AddForce(transform.forward * -8.0f, ForceMode.Impulse);
        ani.SetTrigger("Damaged");
    }

    public void SkillCheck()
    {
        float skilldamage = SkillDamage(skilltableID);
        float targetangle = SetSkillAngle(skilltableID);
        float targetdistance = SetSkillDistance(skilltableID);

        Collider[] colliders = Physics.OverlapSphere(transform.position, targetdistance, 1 << LayerMask.NameToLayer("Enemy"));
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (Collision(colliders[i].transform,targetdistance, targetangle))
            {
                colliders[i].GetComponent<BaseEnemy>().OnDegreadHp(skilldamage);
            }
        }
    }

    public void Check()
    { 
        Collider[] colliders = Physics.OverlapSphere(transform.position, drawLength, 1 << LayerMask.NameToLayer("Enemy"));
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (Collision(colliders[i].transform))
            {
                colliders[i].GetComponent<BaseEnemy>().OnDegreadHp(_stat.Attack);
            }
        }
    }

    public void IgnoreMove()
    {
        Debug.DrawRay(transform.position, transform.forward * 1, Color.green);
        IsWall = Physics.Raycast(transform.position, transform.forward, 1, 1 << LayerMask.NameToLayer("Block"));
    }

    public void Clear()
    {
        followTransform = null;
        _cameraController = null;
    }

    public bool Collision(Transform target,float Length = 3, float Angle = 40)
    {
        Vector3 targetDir = (target.position - transform.position).normalized;

        float targetCos = Vector3.Dot(forwardVec, targetDir);

        float targetAngle = Mathf.Acos(targetCos) * 180 / 3.141596f;

        float distance = Vector3.Distance(transform.position, target.position);

        if (targetAngle <= Angle && distance < Length)
            return true;
        return false;
    }

}
