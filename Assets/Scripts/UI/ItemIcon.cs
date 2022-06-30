using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class ItemIcon : BaseUI,IPointerEnterHandler,IPointerExitHandler
{
    static Inventory inventory;
    static PlayerState State;
    public Image ItemCountImage;
    public Text ItemCount;

    private int m_itemtableId = 0;
    private int m_UniqueId = 0;
    private float m_itemCoolTime = 0;
    private Image m_icon;
    private static Sprite EmptyIcon;

    public static bool dragging = false;
    public static bool dropImage;
    public static bool IsIngameItemslot = false;

    public int ItemTableId { get { return m_itemtableId; } set { m_itemtableId = value; } }
    public int UniqueId { get { return m_UniqueId; } set { m_UniqueId = value; } }
    public float ItemCoolTime { get { return m_itemCoolTime; } set { m_itemCoolTime = value; } }
    public Sprite Icon { get { return m_icon.sprite; } set { m_icon.sprite = value; } }


    public void SetEmptyIcon()
    {
        m_itemtableId = 0;
        m_UniqueId = 0;
        if(m_itemtableId == 0)
            m_icon.sprite = EmptyIcon;
    }
    public static void LoadEmptyIcon()
    {
        EmptyIcon = Resources.Load<Sprite>("Icons/Mini_frame0");
    }
    public void SetInfo(int tableId,int uniqueid)
    {
        m_itemtableId = tableId;
        m_UniqueId = uniqueid;
        if(Item.ItemStatDic.ContainsKey(tableId))
            m_itemCoolTime = Item.ItemStatDic[tableId].CoolTime;
        SetTableIdToSprite(tableId);
        SetItemType(m_itemtableId);
        SetItemUnique();
    }

    public void SetItemUnique()
    {
        if(m_UniqueId > 1)
        {
            if(ItemCountImage != null)
            {
                SetItemCountImgae(true);
                ItemCount.text = m_UniqueId.ToString();
            }
        }
        else if(m_UniqueId == 1)
        {
            if (ItemCountImage != null)
                SetItemCountImgae(false);
        }
        else
        {
            SetEmptyIcon();
        }
    }

    public void SetItemCountImgae(bool state)
    {
        ItemCountImage.gameObject.SetActive(state);
    }

    public void DeleteUnique()
    {
        if (m_UniqueId > 0)
            m_UniqueId--;
        if (m_UniqueId == 0)
        {
            SetEmptyIcon();
        }
        SetItemUnique();
    }

    public void SetActive(bool state)
    {
        m_icon.gameObject.SetActive(state);
    }
    public void SetTableIdToSprite(int itemtableid)
    {
        if (Item.ItemIconDIc.ContainsKey(itemtableid))
        {
            m_icon.sprite = Item.ItemIconDIc[itemtableid];
            return;
        }
        SetEmptyIcon();
    }

    public void SetImage(Image image)
    {
        m_icon = image;
    }

    public bool IsEmepty()
    {
        return m_itemtableId == 0 ? true : false;
    }
    public ItemType SetItemType(int id)
    {
        if (id == 0)
            return ItemType.Null;
        return Item.ItemStatDic[id].Type;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowItemInfo();
    }

    public virtual void ShowItemInfo()
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeactiveItemInfo();
    }

    public virtual void DeactiveItemInfo()
    {

    }

    public void UseItem()
    {
        if(SetItemType(m_itemtableId) == ItemType.Item)
        {
            string name = Item.ItemStatDic[m_itemtableId].Name;
            switch (name)
            {
                case "apple":
                case "hp":
                case "Meat":
                    int itemHP = Item.ItemStatDic[m_itemtableId].HP;
                    _player.AddHp(itemHP);
                    break;
                case "mp":
                    int itemMP = Item.ItemStatDic[m_itemtableId].MP;
                    _player.AddMp(itemMP);
                    break;
            }
        }
    }
}
