using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EffectSoundSetting : SoundSetting
{
    private Slider EffectSlider;
    public TMP_Text text;

    public Image IconOn;
    public Image IconOff;

    public override void Init()
    {
        EffectSlider = GetComponentInChildren<Slider>();
        EffectSlider.value = 0.5f;
        EffectSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SetCurrentSoundText();
    }

    public override void SetCurrentSoundText()
    {
        float value = EffectSlider.value * 10;
        int soundvalue = Mathf.CeilToInt(value);
        text.text = soundvalue.ToString();
        if (EffectSlider.value == 0)
        {
            if (IconOn != null && IconOff != null)
            {
                IconOn.gameObject.SetActive(false);
                IconOff.gameObject.SetActive(true);
            }
        }
        else
        {
            if (IconOn != null && IconOff != null)
            {
                IconOn.gameObject.SetActive(true);
                IconOff.gameObject.SetActive(false);
            }
        }
    }
    public override void SetDefault()
    {
        if (EffectSlider != null)
        {
            EffectSlider.value = 0.5f;
            float EffectVolum = EffectSlider.value;
            GameAudioManager.Instance.SetGameEffectVolum(EffectVolum);
            SetCurrentSoundText();
        }
    }

    private void ValueChangeCheck()
    {
        float EffectVolum = EffectSlider.value;
        GameAudioManager.Instance.SetGameEffectVolum(EffectVolum);
        SetCurrentSoundText();
    }

}
