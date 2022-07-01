using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAndItemSlotGroup : InGameUI
{
    public List<InGameSkillSlot> IngameSkillSlotList = new List<InGameSkillSlot>();
    public List<InGameItemSlot> IngameItemSlotList = new List<InGameItemSlot>();
    public void Set()
    {
        GetSkillSlotList();
        GetItemSlotList();
    }

    public void GetSkillSlotList()
    {
        InGameSkillSlot[] inGameSkillSlots = GetComponentsInChildren<InGameSkillSlot>();
        if(inGameSkillSlots != null)
        {
            for(int i = 0; i < inGameSkillSlots.Length; i++)
            {
                IngameSkillSlotList.Add(inGameSkillSlots[i]);
                IngameSkillSlotList[i].Set();
                _player.HasSkillList.Add(IngameSkillSlotList[i]);
                IngameSkillSlotList[i].SetSkillSlotID(i);
            }
        }
    }

    public void GetItemSlotList()
    {
        InGameItemSlot[] inGameItemSlots = GetComponentsInChildren<InGameItemSlot>();
        if (inGameItemSlots != null)
        {
            for (int i = 0; i < inGameItemSlots.Length; i++)
            {
                IngameItemSlotList.Add(inGameItemSlots[i]);
                IngameItemSlotList[i].Set();
            }
        }
    }

    public void CheckSkillSlotList(int skilltableid)
    {
        for(int i = 0; i < IngameSkillSlotList.Count; i++)
        {
            if(IngameSkillSlotList[i].SkillId == skilltableid)
            {
                IngameSkillSlotList[i].SetEmptyIcon();
            }
        }
    }
}
