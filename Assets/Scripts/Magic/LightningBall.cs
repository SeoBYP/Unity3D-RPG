using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour
{
    Rigidbody rigid;
    CapsuleCollider capsule;
    PlayerController _player;

    public Transform impact;
    public Transform ball;

    private float attack;
    bool IsLightningAttack = false;

    public void Set(float damage)
    {
        rigid = GetComponentInChildren<Rigidbody>();
        capsule = GetComponentInChildren<CapsuleCollider>();
        ball.gameObject.SetActive(false);
        impact.gameObject.SetActive(false);
        _player = BaseUI._player;
        attack = damage;
        IsLightningAttack = true;
    }

    public void MoveBall()
    {
        //transform.position = startPos.position + new Vector3(0,1,1);
        ball.gameObject.SetActive(true);
        rigid.velocity = transform.forward * 7.0f;
        GameAudioManager.Instance.Play2DSound("LightningBall");
    }

    public void ShowImpact()
    {
        ball.gameObject.SetActive(false);
        impact.gameObject.SetActive(true);
    }

    public void DeactiveImpact()
    {
        impact.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_player != null)
        {
            if (IsLightningAttack)
            {
                _player.OnDegreadHp(attack);
                ShowImpact();
                IsLightningAttack = false;
            }
        }
    }

}
