using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReinforCurrentItemStat : MonoBehaviour
{
    public List<Text> textList = new List<Text>();
    public void Init()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            textList.Add(texts[i]);
        }
    }

    public void SetWeaponCurrentStat(int TableID)
    {
        textList[0].text = Item.ItemDataDic[TableID]["Level"];
        textList[1].text = Item.ItemDataDic[TableID]["Attack"];
        textList[2].text = Item.ItemDataDic[TableID]["Critical"];
        textList[3].text = Item.ItemDataDic[TableID]["MaxLevel"];
    }

    public void SetArmorCurrentStat(int TableID)
    {
        textList[0].text = Item.ItemDataDic[TableID]["Level"];
        textList[1].text = Item.ItemDataDic[TableID]["Defence"];
        textList[2].text = Item.ItemDataDic[TableID]["Type"];
        textList[3].text = Item.ItemDataDic[TableID]["MaxLevel"];
    }
}
