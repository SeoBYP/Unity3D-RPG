using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopBuyPopupUI : BaseUI
{
    Canvas canvas;
    public Button ExitButton;
    Inventory _inventory;
    ItemShopContents contents;
    public List<ShopSlot> ShopSlotList = new List<ShopSlot>();
    List<int> ShopSlotTable = new List<int>();
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        ExitButton.onClick.AddListener(Exit);
        contents = GetComponentInChildren<ItemShopContents>();
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        SetItems();
        SetShopslot();
    }

    public void SetItems()
    {
        ShopSlot shopSlot = Resources.Load<ShopSlot>("Prefabs/UI/ShopSlot");
        
        for (int i = 1; i <= Item.ItemDataDic.Count; i++)
        {
            if (Item.ItemDataDic.ContainsKey(i))
            {
                if (Item.ItemDataDic[i]["Type"] == "Item")
                {
                    ShopSlotList.Add(Instantiate<ShopSlot>(shopSlot, contents.transform));
                    ShopSlotTable.Add(i);
                }
            }
        }
        
    }

    public void SetShopslot()
    {
        for (int i = 0; i < ShopSlotList.Count; i++)
        {
            ShopSlotList[i].Init();
            int table = ShopSlotTable[i];
            string Price = Item.ItemDataDic[table]["Price"];
            string Name = Item.ItemDataDic[table]["Name"];
            ShopSlotList[i].GetSprite(Item.ItemIconDIc[table]);
            ShopSlotList[i].GetText(Price, Name, table);
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
        ItemShopUI.IsItemPopupOpen = false;
        _inventory.OpenAndClose();
        IsOpenPopup = false;
    }
}
