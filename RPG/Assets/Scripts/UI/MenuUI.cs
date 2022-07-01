using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI
{
    Canvas canvas;

    public Button ExitButton;
    public Button ReStartButton;
    public Button SaveButton;
    public Button SettingButton;

    public static bool IsMenuOpen = false;

    Inventory _inventory;
    PlayerState _playerState;
    GameSetting _gameSetting;

    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        ExitButton.onClick.AddListener(Exit);
        SaveButton.onClick.AddListener(Save);
        ReStartButton.onClick.AddListener(ReStartGame);
        SettingButton.onClick.AddListener(Setting);
    }

    public void OpenAndClose()
    {
        if (!IsMenuOpen)
        {
            UIManager.Instance.FadeOut();
            Invoke("Open", 1.1f);
        }
    }

    void Open()
    {
        canvas.gameObject.SetActive(true);
        IsMenuOpen = true;
    }

    void Exit()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        //Application.Quit();
    }

    void Save()
    {
        print(Application.persistentDataPath);
        print("Save");

        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        _playerState = UIManager.Instance.Get<PlayerState>(UIList.PlayerState);

        string cahrfilepath = Application.persistentDataPath + "/CharacterInformation";
        string skillfilepath = Application.persistentDataPath + "/SkillInfomation";
        string itemfilepath = Application.persistentDataPath + "/ItemInformation";
        string shopfilepath = Application.persistentDataPath + "/ShopInformaition";
        string inventoryslotpath = Application.persistentDataPath + "/InventorySlotInformation";
        string statuspath = Application.persistentDataPath + "/PlayerStateInformation";
        string questPath = Application.persistentDataPath + "/QuestInformation";

        _playerStat.SaveStatData(cahrfilepath);
        PlayerSkill.SaveSkillStat(skillfilepath);
        Item.SaveItemData(itemfilepath);
        Item.SaveShopData(shopfilepath);
        Quest.SaveQuestData(questPath);
        _inventory.SaveInventorySlot(inventoryslotpath);
        _playerState.SaveState(statuspath);
    }

    void Setting()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        _gameSetting = UIManager.Instance.Get<GameSetting>(UIList.GameSetting);
        _gameSetting.Open();
    }

    void ReStartGame()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        UIManager.Instance.FadeIn();
        canvas.gameObject.SetActive(false);
        IsMenuOpen = false;
    }
}
