using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private new Animation animation;
    private TMP_Text tmpText;

    public bool ActiveSelf{ get {return gameObject.activeSelf;} }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public void Init()
    {
        animation = GetComponentInChildren<Animation>();
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void Execute(Vector3 position, int damage, bool ciritical = false)
    {
        transform.position = Camera.main.WorldToScreenPoint(position);
        if (tmpText != null)
            tmpText.text = damage.ToString();
        float length = 0;
        if(ciritical == false)
        {
            AnimationState state = animation["Normal"];
            if (state != null)
                length = state.length;
            animation.Play("Normal");
        }

        else
        {
            AnimationState state = animation["Critical"];
            if (state != null)
                length = state.length;
            animation.Play("Critical");
        }

        Invoke("Deactive", length);
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
