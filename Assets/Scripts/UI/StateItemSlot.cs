using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StateItemSlot : ItemIcon, IDragHandler, IDropHandler, IEndDragHandler, IBeginDragHandler
{
    private Button slotButton;
    public Image slotIcon;
    public ItemType SlotItemType;
    private static DragAndDropContainer dragContainer;
    public static StatText _statText;
    public Transform ShowitemInfoPos;

    public override void Init()
    {
        slotButton = GetComponent<Button>();
        if (_statText == null)
        {
            _statText = FindObjectOfType<StatText>();
            _statText.GetTextDic();
            _statText.SetDefaultStat(_playerStat);
        }
        Transform t = transform.Find("Slot");
        if (t != null)
        {
            SetImage(t.GetComponent<Image>());
        }
        dragContainer = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
    }

    public void SetStateItemSlotType(ItemType _itemType)
    {
        SlotItemType = _itemType;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmepty())
        {
            dragContainer.Show(true, this);
            DeletePlayerStat(ItemTableId);
            dropImage = false;
            dragging = true;
        }
        else if (IsEmepty())
        {
            return;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragContainer.itemslot == null)
            return;
        dropImage = true;
        if (IsEmepty())
        {
            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            if (!CheckItemType(SetItemType(itemtable)))
            {
                SetEmptyIcon();
                //SetPlayerStat(itemtable);
                dragging = false;
                return;
            }
            if(itemunique > 1)
            {
                dragContainer.itemslot.DeleteUnique();
                int newitemunique = dragContainer.itemslot.UniqueId;
                dragContainer.itemslot.SetInfo(itemtable, newitemunique);
                itemunique = 1;
            }
            SetInfo(itemtable, itemunique);
            SetPlayerStat(itemtable);
            WeaponActive(itemtable);

            dragContainer.SetActive(false);
            int dragitemunique = dragContainer.itemslot.UniqueId;
            if (dragitemunique < 1)
            {
                dragContainer.itemslot.SetEmptyIcon();
                dragContainer.itemslot.SetItemUnique();
                dragContainer.itemslot = null;
                dragging = false;
                return;
            }
            dragContainer.itemslot.SetEmptyIcon();
            dragContainer.itemslot.SetItemUnique();
            dragContainer.itemslot = null;
            
            dragging = false;
        }

        else if (!IsEmepty())
        {
            int prevtableID = ItemTableId;
            int prevuniqueID = UniqueId;

            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            ItemType itemtype = dragContainer.itemslot.SetItemType(itemtable);
            if (!CheckItemType(itemtype))
            {
                SetInfo(prevtableID, prevuniqueID);
                dragging = false;
                return;
            }
            SetInfo(itemtable, itemunique);
            SetPlayerStat(dragContainer.itemslot.ItemTableId);
            WeaponActive(itemtable);

            DeletePlayerStat(prevtableID);
            dragContainer.itemslot.SetInfo(prevtableID,prevuniqueID);
            dragContainer.itemslot = null;
            dragging = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropImage == true)
        {
            dragContainer.SetActive(false);
            dragging = false;
            return;
        }
        else
        {
            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            SetInfo(itemtable,itemunique);
            SetPlayerStat(itemtable);
            dragContainer.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragContainer.Slot.transform.position = eventData.position;
    }

    public void WeaponActive(int table)
    {
        if (Item.ItemStatDic[table].Type == ItemType.Weapon)
        {
            if (_player != null)
            {
                string name = Item.ItemStatDic[table].Name;
                _player.SetWeapon(name);
            }
        }
    }

    public bool CheckItemType(ItemType type)
    {
        if (SlotItemType == type)
        {
            return true;
        }
        string str = "NO";
        _popup.SetActive(true);
        _popup.SetText(str);
        return false;
    }

    public void SetPlayerStat(int tableId)
    {
        ItemType currType = SetItemType(tableId);
        switch (currType)
        {
            case ItemType.Weapon:
                SetWeaponstat(tableId);
                _statText.SetTextStat("AttackText", currType, _playerStat);
                break;
            case ItemType.Head:
            case ItemType.UpperBody:
            case ItemType.LowerBody:
            case ItemType.Hand:
            case ItemType.Shoes:
            case ItemType.Accessories:
                SetDefenceStat(tableId);
                _statText.SetTextStat("DefenceText", currType, _playerStat);
                break;
            case ItemType.Null:
                return;
        }
    }

    public void DeletePlayerStat(int tableId)
    {
        ItemType currType = SetItemType(tableId);
        switch (currType)
        {
            case ItemType.Weapon:
                DeleteWeaponStat(tableId);
                _statText.SetTextStat("AttackText", currType, _playerStat);
                break;
            case ItemType.Head:
            case ItemType.UpperBody:
            case ItemType.LowerBody:
            case ItemType.Hand:
            case ItemType.Shoes:
            case ItemType.Accessories:
                DeletaDefenceStat(tableId);
                _statText.SetTextStat("DefenceText", currType, _playerStat);
                break;
            case ItemType.Null:
                return;
        }
    }


    public void DeleteWeaponStat(int tableID)
    {
        int attack = Item.ItemStatDic[tableID].Attack;
        int critical = Item.ItemStatDic[tableID].Critical;
        _playerStat.Attack -= attack;
        _playerStat.Ciritical -= critical;

    }

    public void DeletaDefenceStat(int tableID)
    {
        int defence = Item.ItemStatDic[tableID].Defence;
        _playerStat.Defence -= defence;
    }

    public void SetWeaponstat(int tableID)
    {
        int attack = Item.ItemStatDic[tableID].Attack;
        int critical = Item.ItemStatDic[tableID].Critical;
        _playerStat.Attack += attack;
        _playerStat.Ciritical += critical;
    }

    public void SetDefenceStat(int tableID)
    {
        int defence = Item.ItemStatDic[tableID].Defence;
        _playerStat.Defence += defence;
    }

    public override void ShowItemInfo()
    {
        if (ItemTableId == 0)
            return;
        ShowItemInfo _showItemInfo = UIManager.Instance.Get<ShowItemInfo>(UIList.ShowItemInfo);
        _showItemInfo.ShowInfo(ShowitemInfoPos.position, ItemTableId, true);
    }

    public override void DeactiveItemInfo()
    {
        ShowItemInfo _showItemInfo = UIManager.Instance.Get<ShowItemInfo>(UIList.ShowItemInfo);
        _showItemInfo.ShowInfo(ShowitemInfoPos.position, ItemTableId, false);
    }

}
