using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ReiforItemSlot : ItemIcon,IDragHandler,IDropHandler,IBeginDragHandler,IEndDragHandler
{
    private Button slotButton;
    public ItemType SlotItemType;
    private static DragAndDropContainer dragContainer;
    public WeaponShopReinfor _weaponReinfor;
    public override void Init()
    {
        slotButton = GetComponent<Button>();
        Transform t = transform.Find("Slot");
        if (t != null)
        {
            SetImage(t.GetComponent<Image>());
            SetActive(false);
        }
        //_weaponReinfor = UIManager.Instance.Get<WeaponShopReinfor>(UIList.WeaponShopReinfor);
        _weaponReinfor = GetComponentInParent<WeaponShopReinfor>();
        dragContainer = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
        //LoadEmptyIcon();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmepty())
        {
            dragContainer.Show(true, this);
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
        dropImage = true;
        if (IsEmepty())
        {
            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            if (CheckItemType(SetItemType(itemtable)) == false)
            {
                SetEmptyIcon();
                return;
            }
            SetInfo(itemtable, itemunique);
            SetActive(true);
            _weaponReinfor.SetProbability(ItemTableId);
            _weaponReinfor.SetCategory(SetItemType(itemtable),ItemTableId);
            _weaponReinfor.SetNeedGold(ItemTableId);

            dragContainer.itemslot.SetEmptyIcon();
            dragging = false;
            return;
        }

        else if (!IsEmepty())
        {
            int prevtableID = ItemTableId;
            int prevuniqueID = UniqueId;

            int itemtable = dragContainer.itemslot.ItemTableId;
            int itemunique = dragContainer.itemslot.UniqueId;
            SetInfo(itemtable, itemunique);
            if (CheckItemType(SetItemType(itemtable)) == false)
            {
                return;
            }
            _weaponReinfor.SetProbability(ItemTableId);
            _weaponReinfor.SetCategory(SetItemType(itemtable), ItemTableId);
            _weaponReinfor.SetNeedGold(ItemTableId);

            dragContainer.itemslot.SetInfo(prevtableID, prevuniqueID);
            dragContainer.itemslot = null;
            dragging = false;
            return;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragContainer.Slot.transform.position = eventData.position;
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
            dragContainer.itemslot.SetInfo(ItemTableId, UniqueId);
            //SetSprite(dragContainer.itemslot.Icon);
            dragContainer.Slot.gameObject.SetActive(false);
        }
    }

    public bool CheckItemType(ItemType type)
    {
        if (type == ItemType.Item)
        {
            string str = "NO";
            _popup.SetActive(true);
            _popup.SetText(str);
            return false;
        }
        return true;
        
    }
}
