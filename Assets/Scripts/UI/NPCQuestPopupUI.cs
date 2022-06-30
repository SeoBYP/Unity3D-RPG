using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCQuestPopupUI : BaseUI
{
    public List<int> NPCQuestList = new List<int>();
    Canvas canvas;

    public TMP_Text QuestName;
    public TMP_Text QuestInfo;
    public TMP_Text Rewardgold;
    public TMP_Text RewardExp;
    public TMP_Text ItemName;
    public Image ItemImage;
    public Button AgreeButton;
    public Button ExitButton;
    public Button ClearQuestButton;

    private NPCController npc;
    private int currentQuestId = 0;
    private bool IsSetQuest = false;
    private bool IsAgreeQuest = false;

    public override void Init()
    {
        Quest.SetQuestInfo();
        canvas = GetComponent<Canvas>();
        AgreeButton.onClick.AddListener(AgreeQuest);
        ExitButton.onClick.AddListener(Exit);
        ClearQuestButton.onClick.AddListener(ClearQuest);
        SetQuestList();
    }

    public void SetQuestList()
    {
        for(int i = 0; i <= Quest.QuestInfoDic.Count; i++)
        {
            if (Quest.QuestInfoDic.ContainsKey(i))
            {
                int questid = Quest.QuestInfoDic[i].ID;
                NPCQuestList.Add(questid);
            }
        }
    }

    public void CheckQuest()
    {
        for(int i = 0; i <= NPCQuestList.Count; i++)
        {
            if (Quest.QuestInfoDic.ContainsKey(i))
            {
                bool DidquestClear = Quest.QuestInfoDic[i].DidClear;
                bool isPlayerHave = Quest.QuestInfoDic[i].IsPlayerHave;
                if (DidquestClear == true)
                    continue;
                if (isPlayerHave)
                {
                    SetReward(i);
                    return;
                }
                else
                {
                    if (IsSetQuest)
                        return;
                    int needplayerlevel = Quest.QuestInfoDic[i].NeedLevel;
                    if(_playerStat.Level >= needplayerlevel)
                    {
                        SetReward(i);
                    }
                }
            }
        }
    }

    public void SetReward(int questtableid)
    {
        if (!Quest.QuestInfoDic.ContainsKey(questtableid))
            return;
        currentQuestId = questtableid;
        string name = Quest.QuestInfoDic[questtableid].Name;
        int rewardgold = Quest.QuestInfoDic[questtableid].RewardGold;
        int rewarditemtableid = Quest.QuestInfoDic[questtableid].RewardItemTable;
        int rewardexp = Quest.QuestInfoDic[questtableid].RewardEXP;

        QuestName.text = name;
        QuestInfo.text = SetQuestInfo(questtableid);
        Rewardgold.text = $"{rewardgold} GOLD";
        RewardExp.text = $"{rewardexp} EXP";
        if (Item.ItemIconDIc.ContainsKey(rewarditemtableid))
        {
            ItemImage.sprite = Item.ItemIconDIc[rewarditemtableid];
        }
        if (Item.ItemStatDic.ContainsKey(rewarditemtableid))
        {
            ItemName.text = Item.ItemStatDic[rewarditemtableid].Name;
        }
        IsSetQuest = true;
    }

    public string SetQuestInfo(int tableid)
    {
        string text = string.Empty;
        string info = Quest.QuestInfoDic[tableid].Info;
        string condition = Quest.QuestInfoDic[tableid].Condition;
        text += "Info : " + info +"\n";
        text += $"Condition : {condition}\n";
        return text;
    }

    public void IsPlayerHaveQuest(int questtableid)
    {
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        if (inGameUI != null)
            inGameUI.SetInGameQuestList(questtableid);

        PlayerQuestPopupUI playerQuest = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);
        if (playerQuest != null)
        {
            playerQuest.SetQuestSlot(questtableid);
            IsAgreeQuest = true;
        }
    }

    public void AgreeQuest()
    {
        if (IsAgreeQuest)
            return;
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        if (inGameUI != null)
            inGameUI.SetInGameQuestList(currentQuestId);

        PlayerQuestPopupUI playerQuest = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);
        if(playerQuest != null)
        {
            playerQuest.SetQuestSlot(currentQuestId);
            IsAgreeQuest = true;
        }

        if (Quest.QuestInfoDic.ContainsKey(currentQuestId))
        {
            Quest.QuestInfoDic[currentQuestId].IsPlayerHave = true;
        }
    }

    public void ClearQuest()
    {
        if (!Quest.QuestInfoDic.ContainsKey(currentQuestId))
            return;
        if (Quest.QuestInfoDic[currentQuestId].IsClear == false)
        {
            PopUPUI();
            return;
        }
        if (Quest.QuestInfoDic.ContainsKey(currentQuestId))
        {
            Quest.QuestInfoDic[currentQuestId].DidClear = true;
        }
        int rewardgold = Quest.QuestInfoDic[currentQuestId].RewardGold;
        int rewarditemtableid = Quest.QuestInfoDic[currentQuestId].RewardItemTable;
        int rewardItemCount = Quest.QuestInfoDic[currentQuestId].RewardItemCount;
        int rewardexp = Quest.QuestInfoDic[currentQuestId].RewardEXP;
        GetReward(rewarditemtableid, rewardItemCount, rewardgold);
        
        InGameUI inGameUI = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        if (inGameUI != null)
            inGameUI.DeactiveIngameQuestList();
        PlayerQuestPopupUI playerQuest = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);
        if (playerQuest != null)
        {
            playerQuest.ClearQuestSlotList();
        }
        if(npc != null)
        {
            npc.CheckQuestClear();

        }
        _player.AddExp(rewardexp);
        IsSetQuest = false;
        IsAgreeQuest = false;
    }

    public void GetReward(int itemtableId, int itemunique,int rewardgold)
    {
        Inventory _inven = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        for (int i = 0; i < _inven.InventorySlot.Count; i++)
        {
            if (_inven.InventorySlot[i].ItemTableId == itemtableId)
            {
                _inven.InventorySlot[i].SetInfo(itemtableId, itemunique);
                _playerStat.AddPlayerGold(rewardgold);
                _inven.SetPlayerGold(_playerStat.Gold);
                currentQuestId = 0;
                return;
            }
            if (_inven.InventorySlot[i].IsEmepty())
            {
                _inven.InventorySlot[i].SetInfo(itemtableId, itemunique);
                _playerStat.AddPlayerGold(rewardgold);
                _inven.SetPlayerGold(_playerStat.Gold);
                currentQuestId = 0;
                return;
            }
        }
    }

    public void PopUPUI()
    {
        if(_popup != null)
        {
            _popup.SetActive(true);
            string text = "Is Not Clear Quest";
            _popup.SetText(text);
        }
    }

    public void Open(NPCController Npc)
    {
        npc = Npc;
        canvas.gameObject.SetActive(true);
    }

    public void Exit()
    {
        canvas.gameObject.SetActive(false);
    }
}
