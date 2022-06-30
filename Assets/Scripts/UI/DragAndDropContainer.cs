using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropContainer : BaseUI
{
    private Canvas canvas;
    public Image Slot;
    public ItemIcon itemslot;
    public SkillIcon skillicon;

    int itemtableid;
    int skilltableid;

    public static bool IsPlayerSkillPopup = false;
    public static bool IsItemPopup = false;

    public void Show(bool state,ItemIcon item)
    {
        itemslot = item;
        itemtableid = item.ItemTableId;
        SetSprite(item.Icon);
        SetActive(state);
    }

    public void SetSprite(Sprite sprite)
    {
        Slot.sprite = sprite;
    }
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        Slot = GetComponentInChildren<Image>();
        Slot.raycastTarget = false;
        Slot.gameObject.SetActive(false);
    }

    public  void SetActive(bool active)
    {
        Slot.gameObject.SetActive(active);
    }

    public void ShowSkill(bool state,SkillIcon skill)
    {
        skillicon = skill;
        skilltableid = skill.SkillId;
        SetSprite(skill.Icon);
        SetActive(state);
    }
}
