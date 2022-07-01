using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ItemShopMenuBtn
{
    ShopBuyBtn,
    ShopSaleBtn,
    ExitBtn,
}

public class ItemShopUI : BaseUI
{
    Canvas canvas;
    public Dictionary<string, Button> MenuBtnDic = new Dictionary<string, Button>();
    ItemShopBuyPopupUI _itemShopBuyPopupUI;
    ItemShopSalePopupUI _itemShopSalePopupUI;
    public static bool IsItemPopupOpen = false;
    public Button ItemShopExitBtn;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        _itemShopBuyPopupUI = UIManager.Instance.Get<ItemShopBuyPopupUI>(UIList.ItemShopBuyPopupUI);
        _itemShopSalePopupUI = UIManager.Instance.Get<ItemShopSalePopupUI>(UIList.ItemShopSalePopupUI);
        ItemShopExitBtn.onClick.AddListener(Exit);
        GetButtons();
    }

    public void Exit()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            IsItemPopupOpen = false;
            IsOpenPopup = false;
        }
    }

    public void GetButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            MenuBtnDic.Add(buttons[i].name, buttons[i]);
            if (buttons[i].name == MenuBtn.ShopBuyBtn.ToString())
                buttons[i].onClick.AddListener(OpenItemShopBuyPopup);
            if (buttons[i].name == MenuBtn.ShopSaleBtn.ToString())
                buttons[i].onClick.AddListener(OpenItemShopSalePopupUI);
        }

    }

    public void OpenItemShopSalePopupUI()
    {
        _itemShopSalePopupUI.Open();
        canvas.gameObject.SetActive(false);
    }

    public void OpenItemShopBuyPopup()
    {
        _itemShopBuyPopupUI.Open();
        canvas.gameObject.SetActive(false);
    }

    public void OpenAndClose()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            canvas.sortingOrder = 0;
            IsItemPopupOpen = false;
            IsOpenPopup = false;
        }
        else
        {
            canvas.gameObject.SetActive(true);
            canvas.sortingOrder = SetSortOrder();
            IsItemPopupOpen = true;
            IsOpenPopup = true;
        }
    }
}
