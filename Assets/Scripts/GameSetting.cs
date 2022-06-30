using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : BaseUI
{
    Canvas canvas;

    List<SoundSetting> sounds = new List<SoundSetting>();

    public Button ExitButton;
    public Button SaveButton;
    public Button DefaultButton;
    public Transform Setting;

    public static bool _isopensetting;
    public static bool IsOpenSetting { get { return _isopensetting; } }

    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 3001;
        SettingInit();
        ExitButton.onClick.AddListener(Exit);
        SaveButton.onClick.AddListener(Save);
        DefaultButton.onClick.AddListener(Default);
    }

    public void SettingInit()
    {
        if(Setting != null)
        {
            SoundSetting[] settings = Setting.GetComponentsInChildren<SoundSetting>();
            for(int i = 0; i < settings.Length; i++)
            {
                sounds.Add(settings[i]);
                settings[i].Init();
            }
        }
    }

    public void Open()
    {
        canvas.gameObject.SetActive(true);
        _isopensetting = true;
    }

    public void Exit()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        canvas.gameObject.SetActive(false);
        _isopensetting = false;
    }

    public void Save()
    {
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        Debug.Log("Save");
    }

    public void Default()
    {
        for(int i = 0; i < sounds.Count; i++)
        {
            if(sounds[i] != null)
            {
                sounds[i].SetDefault();
            }
        }
        GameAudioManager.Instance.PlayButtonSound("ButtonClick");
        Debug.Log("Defualt");
    }
}
