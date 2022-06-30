using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSkillPopupUI : BaseUI
{
    Canvas canvas;
    //SkillSlot skillSlot;
    SkillSlotGroup skillslotgroup;
    static List<SkillSlot> SkillSlotList = new List<SkillSlot>();
    public Button SaveSkillStat;
    public Button ExitButton;
    public static bool IsSaveSkillStat = false;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        skillslotgroup = GetComponentInChildren<SkillSlotGroup>();
        SaveSkillStat.onClick.AddListener(SaveCurSkillStat);
        ExitButton.onClick.AddListener(Exit);
        if (skillslotgroup != null)
        {
            for(int i = 1; i <= PlayerSkill.PlayerSkillStatDic.Count; i++)
            {
                if (PlayerSkill.PlayerSkillStatDic.ContainsKey(i))
                {
                    AddSkillSlot();
                    SkillSlotList[i - 1].SkillTableId = i;
                    SkillSlotList[i - 1].Set();
                    SkillSlotList[i - 1].SetSkillImage(i);
                    SkillSlotList[i - 1].SetSkillInfo(i);
                    SkillSlotList[i - 1].SetSkillActive(i);
                }
            }
        } 
    }

    public void ReSetSkillSlot()
    {
        for(int i = 0; i < SkillSlotList.Count; i++)
        {
            if(SkillSlotList[i] != null)
            {
                SkillSlotList[i].ReSetSkillSlot();
            }
        }
    }

    public bool CheckPlayerLevel(int tableid)
    {
        int SkillLevel = PlayerSkill.PlayerSkillStatDic[tableid].NeedPlayerLevel;
        if(_playerStat.Level >= SkillLevel)
        {
            return true;
        }
        return false;
    }

    public void AddSkillSlot()
    {
        SkillSlot skillSlot = Resources.Load<SkillSlot>("Prefabs/UI/SkillSlot");
        if(skillSlot != null)
        {
            SkillSlotList.Add(Instantiate<SkillSlot>(skillSlot, skillslotgroup.transform));
        }
    }

    public bool IsActive()
    {
        if(_playerStat.SkillStack >= 1)
        {
            return true;
        }
        return false;
    }

    public void ListIsActive()
    {
        for(int i = 0; i < SkillSlotList.Count; i++)
        {
            SkillSlotList[i].IsLevelUP();
        }
    }

    public void OpenAndClose()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            canvas.sortingOrder = ResetSortingOrder();
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
        canvas.gameObject.SetActive(false);
        canvas.sortingOrder = ResetSortingOrder();
        IsOpenPopup = false;
    }

    public void SaveCurSkillStat()
    {
        IsSaveSkillStat = true;
    }
}
