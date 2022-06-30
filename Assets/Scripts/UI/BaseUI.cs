using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum ItemType
{
    Weapon,
    UpperBody,
    LowerBody,
    Shoes,
    Hand,
    Head,
    Accessories,
    Item,
    Null,
}
public class BaseUI : MonoBehaviour
{
    public static PlayerController _player;
    public static PlayerStat _playerStat;
    public static PopupUI _popup;
    public static bool IsOpenPopup = false;

    private ItemType m_itemType;
    private static int uiSortOrder = 0;
    public ItemType ItemType { get { return m_itemType; } set { m_itemType = value; } }
    public int UISortOrder { get { return uiSortOrder; } set { uiSortOrder = value; } }

    public virtual void SetFillAmount( float value )
    {

    }

    public virtual void Init()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerStat = FindObjectOfType<PlayerStat>();
        _popup = UIManager.Instance.Get<PopupUI>(UIList.PopupUI);
        Item.ItemData();
        PlayerSkill.SkillData();
        Stat.StatData();
    } 

    public virtual void ExitBtn()
    {

    }

    public int SetSortOrder()
    {
        uiSortOrder += 1;
        return uiSortOrder;
    }

    public int ResetSortingOrder()
    {
        uiSortOrder = 0;
        return uiSortOrder;
    }
}
