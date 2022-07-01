using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestPopupUI : BaseUI
{
    public List<QuestSlot> QuestSlotList = new List<QuestSlot>();
    Canvas canvas;
    public Transform QuestGroup;
    public Button ExitButton;
    QuestSlot quest;
    int PlayerQuestId = 0;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        LoadQuestSlot();
        ExitButton.onClick.AddListener(Exit);
    }

    public void LoadQuestSlot()
    {
        quest = Resources.Load<QuestSlot>("Prefabs/UI/QuestSlot");
    }

    public void SetQuestSlot(int questid)
    {
        if(quest != null)
        {
            GameData.Instance.PlayerQuestId = questid;
            PlayerQuestId = questid;
            QuestSlot newquestSlot = Instantiate<QuestSlot>(quest, QuestGroup);
            newquestSlot.SetInfo(questid);
            QuestSlotList.Add(newquestSlot);
        }
    }

    public void ReSetQuestSlotList(int questid)
    {
        for(int i = 0; i < QuestSlotList.Count; i++)
        {
            QuestSlotList[i].ReSetQuestSlotCondition(questid);
        }
    }

    public void ClearQuestSlotList()
    {
        for(int i = 0; i < QuestSlotList.Count; i++)
        {
            bool curQuestSlot = QuestSlotList[i].CheckCurQuestSlotID(PlayerQuestId);
            if (curQuestSlot)
            {
                QuestSlotList[i].DeActiveAndDestroy();
                QuestSlotList.RemoveAt(i);
                print(QuestSlotList.Count);
                return;
            }
        }
    }

    public void Exit()
    {
        canvas.gameObject.SetActive(false);
    }

    public void OpenAndClose()
    {
        if (canvas.gameObject.activeSelf)
        {
            Exit();
        }
        else
        {
            canvas.gameObject.SetActive(true);
        }
    }
}
