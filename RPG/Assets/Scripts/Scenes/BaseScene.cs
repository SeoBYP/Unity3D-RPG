using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Stage
{
    TitleScene,
    TownScene,
    FieldScene,
    DungeonScene,
    BattleScene,
}

public class BaseScene : MonoBehaviour
{
    public static PlayerController _player;
    public CameraController _camera;

    public static Stage curstage = Stage.TownScene;
    public static Stage CurrentStage { get { return curstage; } set { curstage = value; } }

    public static Inventory _inventory;
    public static PlayerState _playerState;
    public static PlayerSkillPopupUI _playerSkillPopup;
    public static InGameUI _ingameui;
    public static MenuUI _menu;
    public static FadeUI _fadeUI;
    public static GameSetting _gamesetting;
    public static PlayerQuestPopupUI _playerquestPopup;
    public static GameData gameData;
    protected void MoveToNextScene(Stage stage)
    {
        SceneManager.LoadScene(stage.ToString());
        curstage = stage;
        UIManager.Instance.FadeIn();
        if(_ingameui != null)
        {
            _ingameui.miniMap.MovetoNextMap(stage.ToString());
            _ingameui.SetGameStageInfo(stage);
        }
            
    }

    public virtual void Init()
    {
        if (_player != null)
        {
            _player.Clear();
            _player.Init();
        }
        if (_camera == null)
        {
            _camera = FindObjectOfType<CameraController>();
        }

        if (_camera != null)
        {
            _camera.Clear();
            _camera.Init();
        }
        if (gameData == null)
        {
            gameData = new GameData();
        }

        UIManager.Instance.Add<FadeUI>(UIList.FadeUI, false);
        UIManager.Instance.Add<GameSetting>(UIList.GameSetting, false);
    }

    private void Start()
    {
        Init();
    }

    public void Run()
    {
        SetItemKey();
        if (InputManager.Inventory)
        {
            _inventory.OpenAndClose();
        }
        if (InputManager.PlayerState)
        {
            _playerState.OpenAndClose();
        }
        if (InputManager.SkillPopup)
        {
            _playerSkillPopup.OpenAndClose();
        }
        if (InputManager.Menu)
        {
            _menu.OpenAndClose();
        }
        if (InputManager.QuestPopup)
        {
            _playerquestPopup.OpenAndClose();
        }
    }

    public void SetItemKey()
    {
        if (InputManager.Item1)
            _ingameui.PlayItemCoolTime(0);
        if (InputManager.Item2)
            _ingameui.PlayItemCoolTime(1);
        if (InputManager.Item3)
            _ingameui.PlayItemCoolTime(2);
        if (InputManager.Item4)
            _ingameui.PlayItemCoolTime(3);
        if (InputManager.Item5)
            _ingameui.PlayItemCoolTime(4);
    }
}
