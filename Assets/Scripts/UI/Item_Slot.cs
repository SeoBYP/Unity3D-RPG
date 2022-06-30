using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Slot : ItemIcon,IDragHandler, IDropHandler,IEndDragHandler, IBeginDragHandler
{
    //public ItemType SlotItemType;
    public SkillCoolTime coolTime;

    private Button slotButton;
    private static DragAndDropContainer dragContainer;

    public Transform ShowitemInfoPos;
    public override void Init()
    {
        slotButton = GetComponent<Button>();
        Transform t = transform.Find("Slot");
        if (t != null)
        {
            SetImage(t.GetComponent<Image>());
        }
        dragContainer = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
        slotButton.onClick.AddListener(OnClick);
        if (coolTime != null)
            coolTime.Init();
        LoadEmptyIcon();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmepty())
        {
            dragContainer.Show(true, this);
            //SetEmptyIcon();
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
            if (IsIngameItemslot)
            {
                dragContainer.itemslot.SetEmptyIcon();
                dragContainer.itemslot = null;
                dragging = false;
                return;
            }
            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            SetInfo(itemtable,itemunique);

            dragContainer.SetActive(false);
            dragContainer.itemslot.SetEmptyIcon();
            dragContainer.itemslot.SetItemUnique();
            dragContainer.itemslot = null;
            dragging = false;
        } 
        else if (!IsEmepty())
        {
            int prevtableID = ItemTableId;
            int prevuniqueId = UniqueId;

            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            if (itemtable == ItemTableId)
            {
                UniqueId += itemunique;
                SetInfo(itemtable, itemunique);

                dragContainer.itemslot.SetEmptyIcon();
                dragContainer.itemslot.SetItemUnique();
                dragContainer.itemslot = null;
                dragging = false;
                return;
            }
            SetInfo(itemtable,itemunique);

            dragContainer.itemslot.SetInfo(prevtableID,prevuniqueId);
            dragging = false;
        }
        //SlotItemType = SetItemType();
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragContainer.Slot.transform.position = eventData.position;
    }

    public void OnClick()
    {
        if (dragging)
            return;
        if (!IsEmepty())
        {
            float ItemCoolTime = Item.ItemStatDic[ItemTableId].CoolTime;
            UseItem();
            DeleteUnique();
            coolTime.Execute(ItemCoolTime);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ItemIcon.dropImage == true)
        {
            dragContainer.Slot.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (dragContainer.itemslot != null)
                dragContainer.itemslot.SetInfo(ItemTableId, UniqueId);
            dragContainer.Slot.gameObject.SetActive(false);
        }
    }

    public override void ShowItemInfo()
    {
        if (ItemTableId == 0)
            return;
        ShowItemInfo _showItemInfo = UIManager.Instance.Get<ShowItemInfo>(UIList.ShowItemInfo);
        _showItemInfo.ShowInfo(ShowitemInfoPos.position, this.ItemTableId, true);
    }

    public override void DeactiveItemInfo()
    {
        ShowItemInfo _showItemInfo = UIManager.Instance.Get<ShowItemInfo>(UIList.ShowItemInfo);
        _showItemInfo.ShowInfo(ShowitemInfoPos.position, this.ItemTableId, false);
    }
}
