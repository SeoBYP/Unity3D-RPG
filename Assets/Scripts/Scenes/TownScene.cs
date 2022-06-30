using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    public override void Init()
    {
        DataManager.Load(TableType.CharacterInformation);
        DataManager.Load(TableType.ItemInformation);
        DataManager.Load(TableType.ShopInformaition);
        DataManager.Load(TableType.SkillInfomation);
        DataManager.Load(TableType.InventorySlotInformation);
        DataManager.Load(TableType.PlayerStateInformation);
        DataManager.Load(TableType.QuestInformation);
        if (_player == null)
            LoadPlayer();

        if (_player != null)
            _player.transform.position = transform.position;
        base.Init();

        UIManager.Instance.Add<MenuUI>(UIList.MenuUI, false);
        UIManager.Instance.Add<PopupUI>(UIList.PopupUI, false);
        UIManager.Instance.Add<ShowItemInfo>(UIList.ShowItemInfo, false);
        UIManager.Instance.Add<DragAndDropContainer>(UIList.DragAndDropContainer);
        UIManager.Instance.Add<InGameUI>(UIList.InGameUI);

        UIManager.Instance.Add<PlayerSkillPopupUI>(UIList.PlayerSkillPopupUI, false);
        UIManager.Instance.Add<Inventory>(UIList.Inventory, false);
        UIManager.Instance.Add<PlayerState>(UIList.PlayerState, false);

        _menu = UIManager.Instance.Get<MenuUI>(UIList.MenuUI);
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        _playerState = UIManager.Instance.Get<PlayerState>(UIList.PlayerState);
        _playerSkillPopup = UIManager.Instance.Get<PlayerSkillPopupUI>(UIList.PlayerSkillPopupUI);
        _ingameui = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);

        UIManager.Instance.Add<ItembuyPopup>(UIList.ItembuyPopup, false);
        UIManager.Instance.Add<WeaponShopReinfor>(UIList.WeaponShopReinfor, false);
        UIManager.Instance.Add<WeaponShopSalePopupUI>(UIList.WeaponShopSalePopupUI, false);
        UIManager.Instance.Add<WeaponShopPopupUI>(UIList.WeaponShopPopupUI, false);
        UIManager.Instance.Add<WeaponShopUI>(UIList.WeaponShopUI, false);
        UIManager.Instance.Add<ShowItemInfo>(UIList.ShowItemInfo, false);
        UIManager.Instance.Add<ItemShopBuyPopupUI>(UIList.ItemShopBuyPopupUI, false);
        UIManager.Instance.Add<ItemShopSalePopupUI>(UIList.ItemShopSalePopupUI, false);
        UIManager.Instance.Add<ItemShopUI>(UIList.ItemShopUI, false);
        UIManager.Instance.Add<NPCQuestPopupUI>(UIList.NPCQuestPopupUI, false);
        UIManager.Instance.Add<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI, false);

        CheckIsPlayerhave();

        _playerquestPopup = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);

        GameAudioManager.Instance.PlayBacground("music_candyland");
    }

    public void LoadPlayer()
    {
        _player = Resources.Load<PlayerController>("Prefabs/UnityChan");
        if (_player != null)
        {
            _player = Instantiate(_player, new Vector3(0,0,-3), Quaternion.identity);
            _player.Init();
        }
        DontDestroyOnLoad(_player);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CheckQuestCondition("Test");
        }
        base.Run();
    }

    public void CheckIsPlayerhave()
    {
        NPCQuestPopupUI NPCQuest = UIManager.Instance.Get<NPCQuestPopupUI>(UIList.NPCQuestPopupUI);
        if (NPCQuest != null)
        {
            for (int i = 0; i <= Quest.QuestInfoDic.Count; i++)
            {
                if (Quest.QuestInfoDic.ContainsKey(i))
                {
                    if(Quest.QuestInfoDic[i].IsClear == false)
                    {
                        if (Quest.QuestInfoDic[i].IsPlayerHave)
                        {
                            NPCQuest.IsPlayerHaveQuest(i);
                        }
                    }
                }
            }
        }
    }

    public void CheckQuestCondition(string condition)
    {
        int currentplayerquest = 0;
        foreach (int index in Quest.QuestInfoDic.Keys)
        {
            if (Quest.QuestInfoDic[index].Condition == condition)
            {
                currentplayerquest = Quest.QuestInfoDic[index].ID;
            }
        }

        if (currentplayerquest == GameData.Instance.PlayerQuestId)
        {
            Quest.CheckCondition(condition, currentplayerquest);
            _ingameui.ReSetIngameQuestList(currentplayerquest);
            PlayerQuestPopupUI playerQuest = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);
            if (playerQuest != null)
                playerQuest.ReSetQuestSlotList(currentplayerquest);
        }
    }
}
