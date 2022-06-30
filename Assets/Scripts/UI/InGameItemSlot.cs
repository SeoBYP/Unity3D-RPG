using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameItemSlot : ItemIcon, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SkillCoolTimer timer;
    private SkillCoolTime coolTimeImage;
    private Button button;
    private DragAndDropContainer _container;
    private bool update = false;
    private float ElapsedTIme = 0;
    private ItemIcon inventoryItemIcon = null;

    public Image itemimage;

    public void Set()
    {
        timer = GetComponentInChildren<SkillCoolTimer>(true);
        if (timer != null)
            timer.Init();
        coolTimeImage = GetComponentInChildren<SkillCoolTime>(true);
        if (coolTimeImage != null)
            coolTimeImage.Init();
        button = GetComponentInChildren<Button>();
        if (button != null)
            button.onClick.AddListener(OnClick);
        _container = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
        SetImage(itemimage);
        //isIngameItemSlot = true;
    }

    public void OnClick()
    {
        Execute();
    }

    public void Execute()
    {
        if (IsEmepty())
        {
            return;
        }
        if (button.enabled == false)
            return;
        if (timer != null)
            timer.Execute(ItemCoolTime);
        if (coolTimeImage != null)
            coolTimeImage.Execute(ItemCoolTime);
        if(inventoryItemIcon != null)
        {
            inventoryItemIcon.DeleteUnique();
        }
        UseItem();
        DeleteUnique();
        button.enabled = false;
        update = true;
        ElapsedTIme = 0;
    }

    private void Update()
    {
        if (update)
        {
            ElapsedTIme += Time.deltaTime / ItemCoolTime;
            if (ElapsedTIme > 1.0f)
            {
                button.enabled = true;
                update = false;
                ElapsedTIme = 0;
            }
        }
    }

    public void GetImage(Sprite sprite)
    {
        itemimage.sprite = sprite;
        SetImage(itemimage);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmepty())
        {
            _container.Show(true, this);
            DragAndDropContainer.IsItemPopup = true;
            IsIngameItemslot = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _container.Slot.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (_container.itemslot == null)
            return;
        if(IsEmepty())
        {
            int id = _container.itemslot.ItemTableId;
            int uniqueID = _container.itemslot.UniqueId;
            inventoryItemIcon = _container.itemslot;
            SetInfo(id, uniqueID);
            if (SetItemType(id) != ItemType.Item)
                return;
            GetImage(_container.itemslot.Icon);
            if(DragAndDropContainer.IsItemPopup)
            {
                _container.itemslot.SetEmptyIcon();
                DragAndDropContainer.IsItemPopup = false;
            }
            _container.itemslot = null;
            _container.SetActive(false);
        }
        else if (!IsEmepty())
        {
            int prevtableID = ItemTableId;
            int prevuniqueID = UniqueId;
            Sprite prevSprite = Icon;

            int id = _container.itemslot.ItemTableId;
            int uniqueID = _container.itemslot.UniqueId;
            inventoryItemIcon = _container.itemslot;

            SetInfo(id,uniqueID);
            //SetSprite(_container.itemslot.Icon);
            SetActive(true);

            //itemIcon.Icon = dragContainer.Slot.sprite;
            //_container.itemslot.SetSprite(prevSprite);
            _container.itemslot.SetInfo(prevtableID,prevuniqueID);
            _container.itemslot.SetActive(true);
            _container.itemslot = null;
            DragAndDropContainer.IsItemPopup = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragAndDropContainer.IsItemPopup = false;
        _container.itemslot = null;
        _container.SetActive(false);
    }
}
