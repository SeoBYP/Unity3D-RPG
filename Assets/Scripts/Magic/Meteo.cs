using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour
{
    Collider MeteoCollider;
    public static PlayerController _player;

    float damage;
    bool IsMeteoAttack = false;

    public void Init(float attack)
    {
        MeteoCollider = GetComponent<Collider>();
        _player = BaseUI._player;
        damage = attack;
        IsMeteoAttack = true;
    }

    public void SetActive(bool state)
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_player != null)
        {
            if (IsMeteoAttack)
            {
                _player.OnDegreadHp(damage);
                IsMeteoAttack = false;
            }
        }
    }
}
