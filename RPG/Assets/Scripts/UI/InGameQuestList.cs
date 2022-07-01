using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameQuestList : MonoBehaviour
{
    public TMP_Text QuestName;
    public TMP_Text CurrentQuestCondition;

    public void DeActive()
    {
        this.gameObject.SetActive(false);
    }

    public void SetQuestList(int questid)
    {
        Active();
        if (Quest.QuestInfoDic.ContainsKey(questid))
        {
            QuestName.text = Quest.QuestInfoDic[questid].Name;
            CurrentQuestCondition.text = SetQuestCondition(questid);
        }
    }

    public void ReSetQuestListCondition(int questid)
    {
        if (Quest.QuestInfoDic.ContainsKey(questid))
        {
            CurrentQuestCondition.text = SetQuestCondition(questid);
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

    public void Active()
    {
        this.gameObject.SetActive(true);
    }
}
