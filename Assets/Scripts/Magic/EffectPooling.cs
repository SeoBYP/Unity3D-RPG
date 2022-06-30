using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooling : MonoBehaviour
{
    private Effect[] prefab;
    //
    private static Dictionary<string, Effect> effectDic = new Dictionary<string, Effect>();
    private Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
    private static EffectPooling instance;

    private float damage;
    private PlayerController _player;
    public static EffectPooling Instance
    {
        get { return instance; } 
    }

    public void Init(float enemyattack)
    {
        damage = enemyattack;
        instance = this;
        SetEffectDic();
    }

    private void SetEffectDic()
    {
        prefab = Resources.LoadAll<Effect>("Prefabs/Effect/EnemyEffect");
        for(int i = 0; i < prefab.Length; i++)
        {
            string name = prefab[i].name;
            if (!effectDic.ContainsKey(name))
            {
                effectDic.Add(name, prefab[i]);
            }
            
        }
    }

    public Effect Pooling(string name)
    {
        Effect _effect = null;
        for(int i = 0; i < effects.Count; i++)
        {
            if(effects.ContainsKey(name))
            {
                if (effects[name].ActiveSelf == false)
                {
                    _effect = effects[name];
                    _effect.SetActive(true);
                }
            }
        }

        if(_effect == null)
        {
            Effect effect = Instantiate(InstantiateEffect(name));
            if(effect != null)
            {
                effect.Init(damage);
                effects.Add(name,effect);
            }
            _effect = effect;
        }
        return _effect;
    }

    public Effect InstantiateEffect(string name)
    {
        Effect effect = null;
        if (effectDic.ContainsKey(name))
        {
            effect = effectDic[name];
        }
        return effect;
    }
}
