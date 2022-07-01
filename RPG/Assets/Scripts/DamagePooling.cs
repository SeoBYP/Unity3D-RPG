using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePooling : MonoBehaviour
{
    private DamageText prefab;
    private List<DamageText> textList = new List<DamageText>();
    private static DamagePooling instance;

    public static DamagePooling Instance
    {
        get { return instance; }
    }

    public void Init()
    {
        instance = this;
        prefab = Resources.Load<DamageText>("Prefabs/UI/DamageText");
    }

    public DamageText Pooling()
    {
        DamageText damage = null;
        for(int i = 0; i < textList.Count; i++)
        {
            if (textList[i].ActiveSelf == false)
            {
                damage = textList[i];
                damage.SetActive(true);
            }
        }

        if(damage == null)
        {
            DamageText damageText = Instantiate(prefab, transform);
            if(damageText != null)
            {
                damageText.Init();
                textList.Add(damageText);
            }
            damage = damageText;
        }
        return damage;
    }
}
