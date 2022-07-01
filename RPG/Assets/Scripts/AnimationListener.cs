using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationListener : MonoBehaviour
{
    public List<Transform> slashList = new List<Transform>();
    public Dictionary<string, Transform> PlayerEffectDic = new Dictionary<string, Transform>();
    public Dictionary<string, Transform> EnemyEffectDic = new Dictionary<string, Transform>();

    public ParticleSystem EnemyAttackEffect = null;
    public Transform EffectPos;
    public Transform SlashRoot;
    public Transform SlashEffectTrans;
    public ParticleSystem SlashParticle;

    public Transform EnemyEffectPos;
    public NavMeshAgent agent;

    private EffectPooling effectPooling;

    public float jumpindex = 1;
    public float speed = 3;

    public float testDashPower = 10.0f;

    float PrevTime = 0;
    float DeleteTIme = 5;

    public void Start()
    {
        SetPlayerSlash();
        SetPlayerSkillEffect();
    }

    public void Init(float attackstat)
    {
        effectPooling = GetComponent<EffectPooling>();
        if (effectPooling != null)
            effectPooling.Init(attackstat);
        SetEnemySkillEffect();
    }

    public void ShowPlayerSkill(string name)
    {
        if (!PlayerEffectDic.ContainsKey(name))
            return;
        Transform trans = PlayerEffectDic[name];
        PlayerEffectDic[name].gameObject.SetActive(true);
        StartCoroutine(IDeacitveEffect(trans, name));
    }

    public void PlaySkillSound(string name)
    {
        GameAudioManager.Instance.Play2DSound(name);
    }

    IEnumerator IDeacitveEffect(Transform trans,string name,float delaytime = 2.0f)
    {
        if (name == "Active")
        {
            PlayerController player = GetComponent<PlayerController>();
            float skilltime = PlayerSkill.PlayerSkillStatDic[2].SkillTime;
            if (player != null)
            {
                player.StartAnger();
            }
            yield return new WaitForSeconds(skilltime);
            trans.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(delaytime);
        trans.gameObject.SetActive(false);
    }

    public void SetPlayerSlash()
    {
        SlashRoot = transform.Find("Slash");
        if (SlashRoot != null)
        {
            for (int i = 0; i < SlashRoot.childCount; i++)
            {
                slashList.Add(SlashRoot.GetChild(i));
            }
        }
        if(SlashEffectTrans != null)
        {
            SlashEffectTrans = Instantiate(SlashEffectTrans, Vector3.up * 2000, Quaternion.identity);
            SlashEffectTrans.SetParent(this.transform);
            SlashParticle = SlashEffectTrans.GetComponentInChildren<ParticleSystem>(true);
            SlashParticle.Play();
        }
    }

    public void SetPlayerSkillEffect()
    {
        if(EffectPos != null)
        {
            for(int i = 0; i < EffectPos.childCount; i++)
            {
                string name = EffectPos.GetChild(i).name;
                PlayerEffectDic.Add(name, EffectPos.GetChild(i));
                PlayerEffectDic[name].gameObject.SetActive(false);
            }
        }
    }

    public void FootR()
    {
        string text = BaseScene.CurrentStage + "Walk";
        GameAudioManager.Instance.Play2DSound(text);
    }
    public void FootL()
    {
        string text = BaseScene.CurrentStage + "Walk";
        GameAudioManager.Instance.Play2DSound(text);
    }

    public void PlayerAttackSound(int soundIndex)
    {
        GameAudioManager.Instance.Play2DSound($"PlayerAttack{soundIndex}");
    }

    public void ActiveSkillAttack()
    {
        SendMessage("SkillCheck", SendMessageOptions.DontRequireReceiver);
    }

    public void OnHitEvent()
    {
        SendMessage("Check", SendMessageOptions.DontRequireReceiver);
    }

    public void Shoot()
    {
        SendMessage("ArrowShoot", SendMessageOptions.DontRequireReceiver);
    }

    public void ShootSkill()
    {
        SendMessage("Skill", SendMessageOptions.DontRequireReceiver);
    }

    public void SetEnemySkillEffect()
    {
        EnemyEffectPos = transform.Find("EnemyEffectPos");
        if(EnemyEffectPos != null)
        {
            for(int i = 0; i < EnemyEffectPos.childCount; i++)
            {
                string name = EnemyEffectPos.GetChild(i).name;
                EnemyEffectDic.Add(name, EnemyEffectPos.GetChild(i));
                EnemyEffectDic[name].gameObject.SetActive(false);
            }
        }
    }

    public void EnemyAttactEffect(string name)
    {
        if (!EnemyEffectDic.ContainsKey(name))
            return;
        Transform trans = EnemyEffectDic[name];
        EffectPooling(trans, name);
    }

    public void EffectPooling(Transform trans, string name)
    {
        Effect effect = effectPooling.Pooling(name);
        if (effect != null)
        {
            EnemySound(name);
            effect.Excute(trans, name);
        }
    }

    public void EnemySound(string name)
    {
        GameAudioManager.Instance.Play2DSound(name);
    }

    public void SlashEffect(int index)
    {
        SlashEffectTrans.rotation = slashList[index].rotation;
        SlashEffectTrans.position = slashList[index].position;
        SlashParticle.Play();
    }


}
