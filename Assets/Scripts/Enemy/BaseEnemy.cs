using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Attack,
    MoveToTarget,
    Dead,
    Pase2,
}

public class BaseEnemy : MonoBehaviour
{
    public EnemyStat _stat;
    public static PlayerController _player;
    public Collider enemyCollider;
    public AnimationListener animationListener;
    public GameController gameController;
    public float curHp;
    public EnemyIcon icon;

    public HP_Bar HP;

    public Vector3 StartPos;
    public Vector3 attackPos = Vector3.zero;

    public bool IsDead = false;
    public bool IsGiveEXP = false;

    public bool ActiveSelf { get { return gameObject.activeSelf; } }

    private EnemyState _state = EnemyState.Idle;
    public EnemyState State
    {
        get { return _state; }
        set
        {
            _state = value;
        }
    }
    public virtual void Init()
    {
        if (_player == null)
            _player = FindObjectOfType<PlayerController>();
        if (gameController == null)
            gameController = FindObjectOfType<GameController>();
        else
            return;
    }

    public virtual void SetHP(HP_Bar hp)
    {
        
    }

    public virtual void SetBossHP(BossHpUI boss)
    {

    }

    public virtual void SetHPFillAmount()
    {

    }

    public virtual void Idle()
    {

    }
    public virtual void Attack()
    {

    }

    public virtual void Pase2Attack()
    {

    }
    public virtual void MoveToTarget()
    {

    }

    public virtual void OnDead()
    {

    }

    public virtual void OnDegreadHp(float AttackDamage)
    {

    }

    public virtual void CheckDead()
    {

    }

    public void RemoveEnemyCount()
    {
        if(gameController != null)
        {
            if(icon != null)
                this.icon.Active(false);
            gameController.CheckQuestCondition(this.gameObject.name);
            gameController.CheckQuestCondition("Enemy");
            gameController.CheckClearStage();
        }
    }

    public virtual void StateUpdate()
    {
        switch (State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.MoveToTarget:
                MoveToTarget();
                break;
            case EnemyState.Dead:
                OnDead();
                break;
            case EnemyState.Pase2:
                Pase2Attack();
                break;
        }
    }

    public virtual void Run()
    {
        //icon.SetPosition(this.transform);
        StateUpdate();
    }

}
