using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuBtn
{
    ShopBuyBtn,
    ShopSaleBtn,
    ReinforBtn,
    ExitBtn,
}

public class WeaponShopUI : BaseUI
{
    Canvas canvas;
    public Dictionary<string, Button> MenuBtnDic = new Dictionary<string, Button>();
    WeaponShopPopupUI _weaponShopPopupUI;
    WeaponShopSalePopupUI _weaponShopSalePopupUI;
    WeaponShopReinfor _weaponShopReinforPopupUI;
    public static bool IsPopupOpen = false;
    public Button WeaponShopExitBtn;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        _weaponShopPopupUI = UIManager.Instance.Get<WeaponShopPopupUI>(UIList.WeaponShopPopupUI);
        _weaponShopSalePopupUI = UIManager.Instance.Get<WeaponShopSalePopupUI>(UIList.WeaponShopSalePopupUI);
        _weaponShopReinforPopupUI = UIManager.Instance.Get<WeaponShopReinfor>(UIList.WeaponShopReinfor);
        WeaponShopExitBtn.onClick.AddListener(Exit);
        GetButtons();
    }

    void Exit()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            canvas.sortingOrder = ResetSortingOrder();
            IsPopupOpen = false;
        }
    }

    public void GetButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            MenuBtnDic.Add(buttons[i].name, buttons[i]);
            if (buttons[i].name == MenuBtn.ShopBuyBtn.ToString())
                buttons[i].onClick.AddListener(OpenWeaponShopBuyPopup);
            if (buttons[i].name == MenuBtn.ShopSaleBtn.ToString())
                buttons[i].onClick.AddListener(OpenWeaponShopSalePopupUI);
            if (buttons[i].name == MenuBtn.ReinforBtn.ToString())
                buttons[i].onClick.AddListener(OpenWeaponShopReinforPopupUI);
        }

    }

    public void OpenWeaponShopSalePopupUI()
    {
        _weaponShopSalePopupUI.Open();
        canvas.gameObject.SetActive(false);
    }

    public void OpenWeaponShopBuyPopup()
    {
        _weaponShopPopupUI.Open();
        canvas.gameObject.SetActive(false);
    }

    public void OpenWeaponShopReinforPopupUI()
    {
        _weaponShopReinforPopupUI.Open();
        canvas.gameObject.SetActive(false);
    }

    public void OpenAndClose()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            canvas.sortingOrder = ResetSortingOrder();
            IsPopupOpen = false;
        }
        else
        {
            canvas.gameObject.SetActive(true);
            canvas.sortingOrder = SetSortOrder();
            IsPopupOpen = true;
        }
    }
}
