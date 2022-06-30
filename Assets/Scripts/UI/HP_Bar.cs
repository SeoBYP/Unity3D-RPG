using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    public Slider hp_Bar;
    private bool _IsDead = false;

    public bool ActiveSelf { get { return gameObject.activeSelf; } }
    public bool IsDead { get { return _IsDead; } }

    public void Init()
    {
        hp_Bar = GetComponent<Slider>();
        _IsDead = false;
    }
    public void Execute(Vector3 pos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos + new Vector3(0,1.5f,0));
        transform.position = screenPos;
    }

    public void Active()
    {
        _IsDead = false;
        this.gameObject.SetActive(true);
    }

    public void DeAcitve()
    {
        _IsDead = true;
        this.gameObject.SetActive(false);
    }

    public void SetFilllAmount(float fValue)
    {
        hp_Bar.value = fValue;
    }

    

}
