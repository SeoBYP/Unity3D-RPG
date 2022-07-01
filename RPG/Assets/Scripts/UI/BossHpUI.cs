using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpUI : MonoBehaviour
{
    public Slider bosshpbar;
    public Text bosshptext;

    public void Init()
    {
        bosshpbar = GetComponentInChildren<Slider>();
        bosshptext = GetComponentInChildren<Text>();
    }

    public void SetActive(bool state)
    {
        this.gameObject.SetActive(state);
    }

    public void SetBossHpAmount(float fvalue)
    {
        bosshpbar.value = fvalue;
    }
}
