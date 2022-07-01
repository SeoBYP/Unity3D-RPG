using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InGameUI : BaseUI
{ 
    Canvas canvas;
    public Slider HP_Bar;
    public Slider MP_Bar;
    public Slider Exp_Bar;
    public MiniMap miniMap;
    public SkillAndItemSlotGroup _skillAndItemSlotGroup;
    public Text playerLevelText;
    public TMP_Text HP;
    public TMP_Text MP;

    GameStageInfo gameStageInfo;
    StageClear stageClear;
    InGameQuestList inGameQuest;
    DefeateStage defeate;
    static PlayerIcon playerIcon;
    private DamagePooling damagePooling;

    bool openInven = false;
    public bool IsSetGameStage = false;
    
    public override void Init()
    {
        base.Init();
        canvas = GetComponent<Canvas>();
        miniMap = GetComponentInChildren<MiniMap>();
        if (miniMap != null)
            miniMap.Setting();
        _skillAndItemSlotGroup = GetComponentInChildren<SkillAndItemSlotGroup>();
        if (_skillAndItemSlotGroup != null)
            _skillAndItemSlotGroup.Set();
        gameStageInfo = GetComponentInChildren<GameStageInfo>();
        if (gameStageInfo != null)
            gameStageInfo.DeActive();
        damagePooling = GetComponentInChildren<DamagePooling>();
        if (damagePooling != null)
            damagePooling.Init();
        stageClear = GetComponentInChildren<StageClear>();
        if (stageClear != null)
            stageClear.Inititate();
        inGameQuest = GetComponentInChildren<InGameQuestList>();
        if (inGameQuest != null)
            inGameQuest.DeActive();
        defeate = GetComponentInChildren<DefeateStage>();
        if (defeate != null)
            defeate.Inititate();
        LoadPlayerIcon();
        if (HP.gameObject.activeSelf == false)
            HP.gameObject.SetActive(true);
        SetPlayerLevelText();
        HP.text = $"{_playerStat.HP} / {_playerStat.MaxHP}";
        MP.text = $"{_playerStat.MP} / {_playerStat.MaxMP}";
    }

    public void LoadPlayerIcon()
    {
        if (playerIcon != null)
            return;
        playerIcon = Resources.Load<PlayerIcon>("Prefabs/UI/PlayerIcon");
        Instantiate<PlayerIcon>(playerIcon, new Vector3(0, 5000, 0),Quaternion.identity);
        //playerIcon.Init();
    }

    public void SetGameStageInfo(Stage stage)
    {
        if(stage == Stage.DungeonScene)
        {
            if(GameData.Instance != null)
            {
                if (!IsSetGameStage)
                {
                    GameData.Instance.SetStage();
                    IsSetGameStage = true;
                }
                gameStageInfo.SetActive();
            }
        }
        else
        {
            gameStageInfo.DeActive();
        }
    }

    public void SetHPFillAmount(float fValue)
    {
        HP_Bar.value = fValue;
        HP.text = $"{_playerStat.HP} / {_playerStat.MaxHP}";
    }

    public void SetFillMPAmount(float value)
    {
        MP_Bar.value = value;
        MP.text = $"{_playerStat.MP} / {_playerStat.MaxMP}";
    }

    public void SetFillEXPAmount(float value)
    {
        Exp_Bar.value = value;
    }

    public void PlayItemCoolTime(int ItemKey)
    {
        _skillAndItemSlotGroup.IngameItemSlotList[ItemKey].Execute();
    }

    public void SetPlayerLevelText()
    {
        playerLevelText.text = _playerStat.Level.ToString();
    }

    public void OpenMenu()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        MenuUI menu = UIManager.Instance.Get<MenuUI>(UIList.MenuUI);
        if(menu != null)
        {
            menu.OpenAndClose();
        }
    }

    public void OpenInventory()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        Inventory inven = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        if (inven != null)
        {
            inven.OpenAndClose();
        }
    }
    public void OpenState()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        PlayerState state = UIManager.Instance.Get<PlayerState>(UIList.PlayerState);
        if (state != null)
        {
            state.OpenAndClose();
        }
    }
    public void OpenPlayerSkill()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        PlayerSkillPopupUI skill = UIManager.Instance.Get<PlayerSkillPopupUI>(UIList.PlayerSkillPopupUI);
        if (skill != null)
        {
            skill.OpenAndClose();
        }
    }

    public void OpenPlayerQuest()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        PlayerQuestPopupUI questPopup = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);
        if(questPopup != null)
        {
            questPopup.OpenAndClose();
        }
    }
    
    public void SetInGameQuestList(int questid)
    {
        inGameQuest.SetQuestList(questid);
    }

    public void ReSetIngameQuestList(int questid)
    {
        inGameQuest.ReSetQuestListCondition(questid);
    }

    public void DeactiveIngameQuestList()
    {
        inGameQuest.DeActive();
    }

    public void SetMiniMapIcon(GameController gameController)
    {
        for(int i =  0; i < gameController.enemies.Count; i++)
        {
            Transform enemytrans = gameController.enemies[i].transform;
            gameController.enemies[i].icon = miniMap.IconPooling(gameController, enemytrans);
        }
    }

    public void SetStageClear()
    {
        GameData.Instance.SetStageReword();
        int reword = GameData.Instance.TotalGold;
        Inventory inven = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        _playerStat.AddPlayerGold(reword);
        inven.SetPlayerGold(_playerStat.Gold);
        if (stageClear != null)
            stageClear.Excute(1.0f, true);
    }

    public void DefeateStage()
    {
        WorldUI world = UIManager.Instance.Get<WorldUI>(UIList.WorldUI);
        if(defeate != null)
            defeate.Excute();
        if (world != null)
        {
            world.EnemyHPRemove();
            world.BossHPRemove();
        }
    }
    
}
