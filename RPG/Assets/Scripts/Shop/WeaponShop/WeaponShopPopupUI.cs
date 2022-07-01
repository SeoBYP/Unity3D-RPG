using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WeaponShopPopupUI : BaseUI
{
    Canvas canvas;
    public Button ExitButton;
    Inventory _inventory;
    WeaponShopContents contents;
    public List<ShopSlot> ShopSlotList = new List<ShopSlot>();
    public override void Init()
    { 
        canvas = GetComponent<Canvas>();
        ExitButton.onClick.AddListener(Exit);
        contents = GetComponentInChildren<WeaponShopContents>();
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        SetItems();
    }

    public void SetItems()
    {
        ShopSlot shopSlot = Resources.Load<ShopSlot>("Prefabs/UI/ShopSlot");
        for (int i = 1; i <= Item.ItemDataDic.Count; i++)
        {
            if (Item.ItemDataDic.ContainsKey(i))
            {
                if(Item.ItemDataDic[i]["Type"] != "Item")
                {
                    ShopSlotList.Add(Instantiate<ShopSlot>(shopSlot, contents.transform));
                    int slotid = i - 1;
                    if (ShopSlotList[slotid] != null)
                    {
                        ShopSlotList[slotid].Init();
                        string Price = Item.ItemDataDic[i]["Price"];
                        string Name = Item.ItemDataDic[i]["Name"];
                        int tableId = i;
                        ShopSlotList[slotid].GetSprite(Item.ItemIconDIc[i]);
                        ShopSlotList[slotid].GetText(Price, Name, i);
                    }
                }
            }
        }
    }

    public void Open()
    {
        canvas.gameObject.SetActive(true);
        canvas.sortingOrder = SetSortOrder();
        _inventory.OpenAndClose();
        IsOpenPopup = true;
    }

    void Exit()
    {
        transform.gameObject.SetActive(false);
        canvas.sortingOrder = ResetSortingOrder();
        WeaponShopUI.IsPopupOpen = false;
        _inventory.OpenAndClose();
        IsOpenPopup = false;
    }

    
}
