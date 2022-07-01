using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReinforCategory : MonoBehaviour
{
    public List<Text> textList = new List<Text>();
    public void Init()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        for(int i = 0; i < texts.Length; i++)
        {
            textList.Add(texts[i]);
        }
    }

    public void SetWeaponCategory()
    {
        textList[0].text = "Level";
        textList[1].text = "Attack";
        textList[2].text = "Critical";
        textList[3].text = "MaxLevel";
    }

    public void SetArmorCategory()
    {
        textList[0].text = "Level";
        textList[1].text = "Defence";
        textList[2].text = "Type";
        textList[3].text = "MaxLevel";
    }

}
