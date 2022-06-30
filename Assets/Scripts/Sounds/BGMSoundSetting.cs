using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BGMSoundSetting : SoundSetting
{
    private Slider BGMSlider;
    public TMP_Text text;

    public Image IconOn;
    public Image IconOff;

    public override void Init()
    {
        BGMSlider = GetComponentInChildren<Slider>();
        BGMSlider.value = 0.5f;
        //Slider의 값이 바뀔 때 마다 ValueChangeCheck 이벤트 함수를 호출 할 수 있도록 선언한다. 
        BGMSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SetCurrentSoundText();
    }

    public override void SetCurrentSoundText()
    {
        float value = BGMSlider.value * 10;
        int soundvalue = Mathf.CeilToInt(value);
        text.text = soundvalue.ToString();
        if(BGMSlider.value == 0)
        {
            if(IconOn != null && IconOff != null)
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
         if(BGMSlider != null)
        {
            BGMSlider.value = 0.5f;
            float BGMVolum = BGMSlider.value;
            GameAudioManager.Instance.SetGameBGMVolum(BGMVolum);
            SetCurrentSoundText();
        }
    }
    //
    private void ValueChangeCheck()
    {
        float BGMVolum = BGMSlider.value;
        GameAudioManager.Instance.SetGameBGMVolum(BGMVolum);
        SetCurrentSoundText();
    }
}
