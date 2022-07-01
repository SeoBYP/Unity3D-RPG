using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PopupUI : BaseUI,IPointerDownHandler
{
    Canvas canvas;
    Image image;
    Text text;

    public static bool IsPopupOpen = false;

    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
    }

    public void SetActive(bool State)
    {
        //canvas.sortingOrder = SetSortOrder();
        canvas.gameObject.SetActive(State);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(canvas.gameObject.activeSelf)
            canvas.gameObject.SetActive(false);
    }

    public void SetText(string str)
    {
        text.text = str;
    }
}
