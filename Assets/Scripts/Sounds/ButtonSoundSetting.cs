using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSoundSetting : SoundSetting
{
    private Slider ButtonSlider;
    public TMP_Text text;

    public override void Init()
    {
        ButtonSlider = GetComponentInChildren<Slider>();
        ButtonSlider.value = 0.5f;
        ButtonSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SetCurrentSoundText();
    }

    public override void SetCurrentSoundText()
    {
        float value = ButtonSlider.value * 10;
        int soundvalue = Mathf.CeilToInt(value);
        text.text = soundvalue.ToString();
    }
    public override void SetDefault()
    {
        if (ButtonSlider != null)
        {
            ButtonSlider.value = 0.5f;
            float EffectVolum = ButtonSlider.value;
            GameAudioManager.Instance.SetGameButtonVolum(EffectVolum);
            SetCurrentSoundText();
        }
    }

    private void ValueChangeCheck()
    {
        float EffectVolum = ButtonSlider.value;
        GameAudioManager.Instance.SetGameButtonVolum(EffectVolum);
        SetCurrentSoundText();
    }
}
