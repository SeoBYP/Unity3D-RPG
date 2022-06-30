using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    Collider m_collider;
    NPCQuestPopupUI NPCQuest;

    public Transform ClearQuest;
    public Transform HaveQuest;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        m_collider = GetComponent<Collider>();
        SetClearQuestDeactive();
        CheckQuestClear();
    }

    public void OnTriggerStay(Collider other)
    {
        if (InputManager.CommunityActive)
        {
            OpenQuestPopup();
        }
    }

    public void OpenQuestPopup()
    {
        NPCQuest = UIManager.Instance.Get<NPCQuestPopupUI>(UIList.NPCQuestPopupUI);
        if (NPCQuest != null)
        {
            NPCQuest.Open(this);
            NPCQuest.CheckQuest();
        }
    }

    public void CheckQuestClear()
    {
        for(int i = 0; i <= Quest.QuestInfoDic.Count; i++)
        {
            if (Quest.QuestInfoDic.ContainsKey(i))
            {
                if (Quest.QuestInfoDic[i].DidClear == true)
                {
                    continue;
                }
                if(Quest.QuestInfoDic[i].IsClear == true)
                {
                    SetClaerQuestAcitive();
                    SethaveQuestDeacitve();
                }
                else
                {
                    SetClearQuestDeactive();
                    SethaveQuestAcitve();
                }
            }
        }        
    }

    public void SetClaerQuestAcitive()
    {
        if (ClearQuest != null)
        {
            ClearQuest.gameObject.SetActive(true);
        }
    }

    public void SetClearQuestDeactive()
    {
        if (ClearQuest != null)
        {
            ClearQuest.gameObject.SetActive(false);
        }
    }

    public void SethaveQuestAcitve()
    {
        if(HaveQuest != null)
        {
            HaveQuest.gameObject.SetActive(true);
        }
    }

    public void SethaveQuestDeacitve()
    {
        if (HaveQuest != null)
        {
            HaveQuest.gameObject.SetActive(false);
        }
    }
}
