using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private string _name = string.Empty;
    public bool ActiveSelf { get { return gameObject.activeSelf; } }
    private LightningBall lightningBall;
    private Meteo meteo;
    private float damage;
    private PlayerController _player;
    private Transform AcitiveTrans;
    private bool IsActive = false;

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public void Init(float attackDamage)
    {
        damage = attackDamage;
        //_player = player;
        meteo = GetComponentInChildren<Meteo>();
        lightningBall = GetComponentInChildren<LightningBall>();
    }

    public void Excute(Transform starttrans,string effectname)
    {
        transform.position = starttrans.position;
        transform.rotation = starttrans.rotation;
        StartCoroutine(DeactiveEffect(starttrans,effectname));
    }

    IEnumerator DeactiveEffect(Transform pos,string name, float delayTime = 2.0f)
    {
        if (name == "Active")
        {
            AcitiveTrans = pos;
            IsActive = true;
            yield return new WaitForSeconds(10.0f);
            IsActive = false;
            transform.gameObject.SetActive(false);
        }
        if (name == "2Pase")
        {
            yield return new WaitForSeconds(12.0f);
            transform.gameObject.SetActive(false);
        }
        if (name == "LightningBall")
        {
            lightningBall.Set(damage);
            lightningBall.MoveBall();
            yield return new WaitForSeconds(1.0f);
            lightningBall.ShowImpact();
            //yield return new WaitForSeconds(1.0f);
            //lightningBall.DeactiveImpact();
        }
        if(name == "Meteo")
        {
            meteo.Init(damage);
        }
        yield return new WaitForSeconds(delayTime);
        transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsActive)
        {
            transform.position = AcitiveTrans.position;
        }
    }

}
