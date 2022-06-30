using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : BaseScene
{
    // Start is called before the first frame update
    public override void Init()
    {
        GameAudioManager.Instance.LoadSounds();
        GameAudioManager.Instance.PlayBacground("Wistful_Woods_Version_02_LOOP");

        Util.BindingFunc(transform, "ButtonGroup/StartButton", SetPlay);
        Util.BindingFunc(transform, "ButtonGroup/ExitButton", SetExit);
        Util.BindingFunc(transform, "ButtonGroup/SettingButton", Setting);
    }

    void SetPlay()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        MoveToNextScene(Stage.TownScene);
    }

    private void SetExit()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        Debug.Log("Exit");
        OnApplicationQuit();
    }

    private void Setting()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        _gamesetting = UIManager.Instance.Get<GameSetting>(UIList.GameSetting);
        _gamesetting.Open();
        Debug.Log("Setting");
    }

    private void OnApplicationQuit()
    {
        
    }
}
