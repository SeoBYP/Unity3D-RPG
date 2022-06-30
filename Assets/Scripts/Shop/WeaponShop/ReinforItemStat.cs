using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReinforItemStat : MonoBehaviour
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

    public void SetWeaponReinforStat(int TableID)
    {
        textList[0].text = "+1";
        textList[1].text = $"+{Item.ItemDataDic[TableID]["AddAttack"]}";
        textList[2].text = $"+{Item.ItemDataDic[TableID]["AddCritical"]}";
        textList[3].text = Item.ItemDataDic[TableID]["MaxLevel"];
    }

    public void SetArmorReinforStat(int TableID)
    {
        textList[0].text = "+1";
        textList[1].text = $"+{Item.ItemDataDic[TableID]["AddDefence"]}";
        textList[2].text = Item.ItemDataDic[TableID]["Type"];
        textList[3].text = Item.ItemDataDic[TableID]["MaxLevel"];
    }
}
