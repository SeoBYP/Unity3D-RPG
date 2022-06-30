using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform SKillEffect;

    Collider _collider;
    Rigidbody rigid;
    PlayerController player;

    private float Damage;
    private bool OnPlayer;
    private bool IsShoot;

    public bool ActiveSelf { get { return gameObject.activeSelf; } }

    public void Init()
    {
        _collider = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        SKillEffect.gameObject.SetActive(false);
    }

    public void SetPlayer(PlayerController obj)
    {
        if(player == null)
            player = obj;
    }

    public void SetActive(bool state)
    {
        transform.gameObject.SetActive(state);
    }

    public void Execute(Transform trans,Vector3 startpos,float damage,bool activeSkill,bool criitical = false)
    {
        transform.position = startpos;
        transform.rotation = trans.rotation;
        // new Vector3(0,90,0);//trans.rotation * 90;//.LookAt(trans.forward);//rotation = //Quaternion.LookRotation(player.transform.position);
        //transform.LookAt(player.transform.position); //trans.rotation;//Quaternion.LookRotation(trans.forward);//Prent.forward;
        Damage = damage;
        if (activeSkill)
        {
            StartCoroutine(SkillShooter(trans));
        }
        StartCoroutine(Shooter(trans));
    }

    IEnumerator Shooter(Transform trans)
    {
        rigid.velocity = trans.forward * 10;
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

    IEnumerator SkillShooter(Transform trans)
    {
        rigid.velocity = trans.forward * 10;
        SKillEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Deactive();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(player != null)
            {
                player.OnDegreadHp(Damage);
                Deactive();
            }
            return;
        }
        
    }

    public void Deactive()
    {
        SKillEffect.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
