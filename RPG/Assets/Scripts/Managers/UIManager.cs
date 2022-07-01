using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UIList
{
    InGameUI,
    WorldUI,
    Inventory,
    PlayerState,
    DragAndDropContainer,
    WeaponShopUI,
    WeaponShopMenuPopup,
    WeaponShopPopupUI,
    WeaponShopSalePopupUI,
    ItembuyPopup,
    PopupUI,
    ShowItemInfo,
    WeaponShopReinfor,
    ItemShopUI,
    ItemShopBuyPopupUI,
    ItemShopSalePopupUI,
    PlayerSkillPopupUI,
    MenuUI,
    FadeUI,
    GameSetting,
    NPCQuestPopupUI,
    PlayerQuestPopupUI,
}

public class UIManager : Manager<UIManager>
{
    Dictionary<UIList, BaseUI> UIDic = new Dictionary<UIList, BaseUI>();
    //BaseUI baseUI;
    public const string Path = "Prefabs";

    public void Add<T>(UIList uIList,bool active = true) where T : BaseUI
    {
        if(UIDic.ContainsKey(uIList) == false)
        {
            T newobject = Instantiate(Resources.Load<T>($"Prefabs/UI/{uIList}"), transform);
            newobject.Init();
            
            newobject.gameObject.SetActive(active);
            UIDic.Add(uIList,newobject);
        }
    }

    public T Get<T>(UIList ui) where T : BaseUI
    {
        if (UIDic.ContainsKey(ui))
        {
            return UIDic[ui] as T;
        }
        return null;
    }

    public void Destroy(UIList ui)
    {
        if (UIDic.ContainsKey(ui) && UIDic[ui] != null)
        {
            Destroy(UIDic[ui].gameObject);
            UIDic.Remove(ui);
        }
    }

    public bool ConstainKey(UIList ui)
    {
        if (UIDic.ContainsKey(ui)) { return true; }
        return false;
    }

    public void FadeIn(float targetTime = 1.0f)
    {
        FadeUI uifade = Get<FadeUI>(UIList.FadeUI);
        if (uifade != null)
            uifade.FadeIn(targetTime);
    }

    public void FadeOut(float targetTime = 1.0f)
    {
        FadeUI uifade = Get<FadeUI>(UIList.FadeUI);
        if (uifade != null)
            uifade.FadeOut(targetTime);
    }
}
