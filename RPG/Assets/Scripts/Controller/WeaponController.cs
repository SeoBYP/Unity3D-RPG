using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    enum Weapon
    {
        Shotsword,
        KatanaBlade,
        Bastardsword,
    }

    public Transform SwordPos;
    public Transform KatanaPos;

    public Dictionary<string, Transform> weaponDic = new Dictionary<string, Transform>();

    public void Init()
    {
        SetWeaponDic();
    }

    public void SetWeaponDic()
    {
        if(SwordPos != null)
        {
            for(int i = 0; i < SwordPos.childCount; i++)
            {
                string name = SwordPos.GetChild(i).gameObject.name;
                if (!weaponDic.ContainsKey(name))
                {
                    weaponDic.Add(name, SwordPos.GetChild(i));
                    if (name == "Shotsword")
                        weaponDic[name].gameObject.SetActive(true);
                    else
                        weaponDic[name].gameObject.SetActive(false);
                }
            }
        }
        if(KatanaPos != null)
        {
            for(int i = 0; i < KatanaPos.childCount; i++)
            {
                string name = KatanaPos.GetChild(i).gameObject.name;
                if (!weaponDic.ContainsKey(name))
                {
                    weaponDic.Add(name, KatanaPos.GetChild(i));
                    weaponDic[name].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ActiveWeapon(string name)
    {
        foreach(string weaponname in weaponDic.Keys)
        {
            if(weaponname == name)
            {
                weaponDic[weaponname].gameObject.SetActive(true);
            }
            else
            {
                weaponDic[weaponname].gameObject.SetActive(false);
            }
        }
        GameAudioManager.Instance.Play2DSound("Swap");
    }
}
