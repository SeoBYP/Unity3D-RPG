using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Inventory : BaseUI
{
    public Dictionary<int, Item_Slot> InventorySlot = new Dictionary<int, Item_Slot>();
    Canvas canvas;
    Image _InventoryPopup;
    public Transform showItemPos;
    public Button _InventoryExitBtn;
    public Text CoinText;

    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        _InventoryPopup = GetComponentInChildren<Image>();
       
        SetPlayerGold(_playerStat.Gold);
        Item_Slot[] _itemSlot = GetComponentsInChildren<Item_Slot>();
        if (_itemSlot != null)
        {
            for(int i = 0; i < _itemSlot.Length; i++)
            {
                InventorySlot.Add(i, _itemSlot[i]);
                _itemSlot[i].Init();
                SetInventorySlotInfo(i);
            }
        }
        _InventoryExitBtn.onClick.AddListener(Exit);
    }

    public void SetInventorySlotInfo(int id)
    {
        if (DataManager.TableDic.ContainsKey(TableType.InventorySlotInformation))
        {
            int defaultItemtableid = DataManager.ToInter(TableType.InventorySlotInformation, id, "ItemTableID");
            int defalutUniqueid = DataManager.ToInter(TableType.InventorySlotInformation, id, "UniqueId");
            InventorySlot[id].SetInfo(defaultItemtableid, defalutUniqueid);
            InventorySlot[id].SetTableIdToSprite(defaultItemtableid);
        }
    }

    public void SetPlayerGold(int Coin)
    {

        CoinText.text = Coin.ToString();
        
    }
    public void OpenAndClose()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            canvas.sortingOrder = ResetSortingOrder();
            Item_Slot.dragging = false;
            IsOpenPopup = false;
        }
        else
        {
            canvas.sortingOrder = SetSortOrder();
            canvas.gameObject.SetActive(true);
            IsOpenPopup = true;
        }
    }

    public void Exit()
    {
        print("Exit");
        canvas.gameObject.SetActive(false);
        canvas.sortingOrder = ResetSortingOrder();
        IsOpenPopup = false;
    }

    public override string ToString()
    {
        string text = string.Empty;
        text += "ID,ItemTableID,UniqueId\n";
        for(int i = 0; i <= InventorySlot.Count; i++)
        {
            if (InventorySlot.ContainsKey(i))
            {
                string newid = i.ToString();
                string newitemtableid = InventorySlot[i].ItemTableId.ToString();
                string newuniqueid = InventorySlot[i].UniqueId.ToString();
                text += $"{newid},{newitemtableid},{newuniqueid}\n";
            }
        }
        return text;
    }

    public void SaveInventorySlot(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffers = System.Text.Encoding.UTF8.GetBytes(ToString());
        fs.Write(buffers, 0, buffers.Length);
        fs.Close();
    }
}
