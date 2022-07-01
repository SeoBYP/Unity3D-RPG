using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatText : MonoBehaviour
{
    public Dictionary<string, Text> statTextDic = new Dictionary<string, Text>();

    public void GetTextDic()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            statTextDic.Add(texts[i].name, texts[i]);
        }
    }

    public void SetDefaultStat(PlayerStat stat)
    {
        for(int i = 0; i < statTextDic.Count; i++)
        {
            statTextDic["AttackText"].text = stat.Attack.ToString();
            statTextDic["CriticalText"].text = stat.Ciritical.ToString();
            statTextDic["DefenceText"].text = stat.Defence.ToString();
            statTextDic["SpeedText"].text = stat.Speed.ToString();
            statTextDic["MaxExpText"].text = stat.MaxExp.ToString();
        }
    }

    public void SetTextStat(string Textname,ItemType type,PlayerStat stat)
    {
        if (statTextDic.ContainsKey(Textname))
        {
            switch (type)
            {
                case ItemType.Weapon:
                    statTextDic[Textname].text = stat.Attack.ToString();
                    break;
                case ItemType.Head:
                case ItemType.UpperBody:
                case ItemType.LowerBody:
                case ItemType.Hand:
                case ItemType.Shoes:
                case ItemType.Accessories:
                    statTextDic[Textname].text = stat.Defence.ToString();
                    break;
            }
            
        }
    }
}
