using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    private Image Mapimage;

    public Sprite MapIcon { get { return Mapimage.sprite; } set { Mapimage.sprite = value; } }
    public string transName { get { return transform.gameObject.name; } }

    public Vector2 SizeDelta
    {
        get
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
                return rectTransform.sizeDelta;
            return Vector2.zero;
        }
    }

    public void Init()
    {
        Mapimage = GetComponent<Image>();
        //Mapimage.size
    }

}
