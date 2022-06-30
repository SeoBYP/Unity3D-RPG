using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShowItemInfo : BaseUI
{
    Canvas canvas;
    public Image Popup;
    public Image ItemSprite;
    public TMP_Text ItemName;
    public TMP_Text Infotext;

    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        Popup = GetComponentInChildren<Image>();
    }

    public void ShowInfo(Vector3 mousePos,int ItemTableId,bool state)
    {
        SetActive(state);
        SetTransform(mousePos);
        SetText(ItemTableId);
        SetSprite(ItemTableId);
    }

    public void SetSprite(int itemtableId)
    {
        if(Item.ItemIconDIc.ContainsKey(itemtableId))
            ItemSprite.sprite = Item.ItemIconDIc[itemtableId];
    }

    public void SetTransform(Vector3 pos)
    {
        Popup.transform.position = pos;
    }
    public void SetActive(bool State)
    {
        canvas.gameObject.SetActive(State);
    }

    public void SetText(int itemtableid)
    {
        if (Item.ItemDataDic.ContainsKey(itemtableid))
        {
            CheckItemType(itemtableid);
        }
            
    }

    private void CheckItemType(int itemtableid)
    {
        ItemType type = Item.ItemStatDic[itemtableid].Type;
        switch (type)
        {
            case ItemType.Weapon:
                SetWeaponInfo(itemtableid);
                break;
            case ItemType.UpperBody:
            case ItemType.LowerBody:
            case ItemType.Shoes:
            case ItemType.Hand:
            case ItemType.Head:
            case ItemType.Accessories:
                SetDefenceItemInfo(itemtableid);
                break;
            case ItemType.Item:
                SetItemInfo(itemtableid);
                break;
        }
    }

    public void SetWeaponInfo(int itemtable)
    {
        ItemName.text = Item.ItemDataDic[itemtable]["Name"];
        string text = string.Empty;
        text += $"Level : {Item.ItemStatDic[itemtable].Level}\n";
        text += $"Attack : {Item.ItemStatDic[itemtable].Attack}\n";
        text += $"Critical : {Item.ItemStatDic[itemtable].Critical}\n";
        text += $"Probability : {Item.ItemStatDic[itemtable].Probability}\n";
        text += $"MaxLevel : {Item.ItemStatDic[itemtable].MaxLevel}\n";

        Infotext.text = text;
    }

    public void SetDefenceItemInfo(int itemtable)
    {
        ItemName.text = Item.ItemDataDic[itemtable]["Name"];
        string text = string.Empty;
        text += $"Level : {Item.ItemStatDic[itemtable].Level}\n";
        text += $"Defence : {Item.ItemStatDic[itemtable].Defence}\n";
        text += $"Probability : {Item.ItemStatDic[itemtable].Probability}\n";
        text += $"MaxLevel : {Item.ItemStatDic[itemtable].MaxLevel}\n";

        Infotext.text = text;
    }

    public void SetItemInfo(int itemtable)
    {
        ItemName.text = Item.ItemDataDic[itemtable]["Name"];
        string text = string.Empty;
        text += $"MP : {Item.ItemStatDic[itemtable].MP}\n";
        text += $"HP : {Item.ItemStatDic[itemtable].HP}\n";
        Infotext.text = text;
    }
}
