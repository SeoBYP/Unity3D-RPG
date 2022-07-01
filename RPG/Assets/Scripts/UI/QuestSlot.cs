using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public TMP_Text questname;
    public TMP_Text questinfo;
    public TMP_Text questcondition;
    public TMP_Text itemname;
    public TMP_Text gold;
    public TMP_Text exp;
    public Image ItemIcon;
    int QuestTableId;

    public void SetInfo(int questid)
    {
        if (QuestTableId == questid)
            return;
        QuestTableId = questid;
        if (Quest.QuestInfoDic.ContainsKey(questid))
        {
            int itemtableid = Quest.QuestInfoDic[questid].ID;
            questname.text = Quest.QuestInfoDic[questid].Name;
            questinfo.text = Quest.QuestInfoDic[questid].Info;
            questcondition.text = SetQuestCondition(questid);
            gold.text = Quest.QuestInfoDic[questid].RewardGold + " GOLD";
            exp.text = Quest.QuestInfoDic[questid].RewardEXP + " EXP";
            if (Item.ItemStatDic.ContainsKey(itemtableid))
                itemname.text = Item.ItemStatDic[itemtableid].Name;
            if (Item.ItemIconDIc.ContainsKey(itemtableid))
                ItemIcon.sprite = Item.ItemIconDIc[itemtableid];
        }
    }

    public void ReSetQuestSlotCondition(int questid)
    {
        if (QuestTableId != questid)
            return;
        if (Quest.QuestInfoDic.ContainsKey(questid))
        {
            questcondition.text = SetQuestCondition(questid);
        }
    }

    public string SetQuestCondition(int questid)
    {
        string text = string.Empty;
        if (Quest.QuestInfoDic.ContainsKey(questid))
        {
            string newcondition = Quest.QuestInfoDic[questid].Condition;
            int currentconditoncount = Quest.QuestInfoDic[questid].CurrentConditionCount;
            int conditioncount = Quest.QuestInfoDic[questid].ConditionCount;

            text += $"{newcondition} {currentconditoncount} / {conditioncount}"; 
        }
        return text;
    }

    public bool CheckCurQuestSlotID(int questid)
    {
        if (QuestTableId == questid)
            return true;
        return false;
    }

    public void DeActiveAndDestroy()
    {
        Destroy(this.gameObject);
    }
}
