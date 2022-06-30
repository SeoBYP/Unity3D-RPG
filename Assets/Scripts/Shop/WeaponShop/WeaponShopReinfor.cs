using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponShopReinfor : BaseUI
{
    Canvas canvas;
    ReiforItemSlot ReiforItem;
    public Button ExitButton;
    public Button ReinforBtn;
    public Text PropabilityText;
    public Text GoldText;
    int NeedReinforGold = 0;
    private int TableID = 0;

    ReinforCategory reinforCategory;
    ReinforCurrentItemStat reinforCurrentItemStat;
    ReinforItemStat reinforItemStat;
    Inventory _inventory;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        ExitButton.onClick.AddListener(Exit);
        ReinforBtn.onClick.AddListener(Reinfor);
        ReiforItem = GetComponentInChildren<ReiforItemSlot>();
        if (ReiforItem != null)
            ReiforItem.Init();
        reinforCategory = GetComponentInChildren<ReinforCategory>();
        if (reinforCategory != null)
            reinforCategory.Init();
        reinforCurrentItemStat = GetComponentInChildren<ReinforCurrentItemStat>();
        reinforCurrentItemStat.Init();
        reinforItemStat = GetComponentInChildren<ReinforItemStat>();
        reinforItemStat.Init();
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
    }

    public void SetNeedGold(int TableId)
    {
        string Text = Item.ItemDataDic[TableId]["ReinforGold"];
        NeedReinforGold = Item.ItemStatDic[TableId].ReinforGold;
        GoldText.text = Text;
    }

    public bool CheckPlayerGold()
    {
        if(_playerStat.Gold >= NeedReinforGold)
        {
            return true;
        }
        return false;
    }

    public void SetProbability(int TableId)
    {
        string probability = Item.ItemDataDic[TableId]["Probability"];
        PropabilityText.text = $"{probability}%";
    }

    public void SetCategory(ItemType type,int tableid)
    {
        switch (type)
        {
            case ItemType.Weapon:
                reinforCategory.SetWeaponCategory();
                reinforCurrentItemStat.SetWeaponCurrentStat(tableid);
                reinforItemStat.SetWeaponReinforStat(tableid);
                break;
            case ItemType.UpperBody:
            case ItemType.LowerBody:
            case ItemType.Shoes:
            case ItemType.Hand:
            case ItemType.Head:
            case ItemType.Accessories:
                reinforCategory.SetArmorCategory();
                reinforCurrentItemStat.SetArmorCurrentStat(tableid);
                reinforItemStat.SetArmorReinforStat(tableid);
                break;
        }
    }

    public void Reinfor()
    {
        TableID = ReiforItem.ItemTableId;
        if (!Item.ItemStatDic.ContainsKey(TableID))
            return;
        ItemType = ReiforItem.SetItemType(TableID);
        int itemCurLevel = Item.ItemStatDic[TableID].Level;
        int itemMaxLevel = Item.ItemStatDic[TableID].MaxLevel;
        if (CheckPlayerGold())
        {
            if (itemCurLevel >= itemMaxLevel)
            {
                string str = "It Has MaxLevel";
                _popup.SetActive(true);
                _popup.SetText(str);
                return;
            }
            else
            {
                float rnd = Random.Range(0, 100);

                if (rnd < Item.ItemStatDic[TableID].Probability)
                {
                    Item.AddItemStat(TableID);
                    SetCategory(ItemType, TableID);
                    SetProbability(TableID);

                    string str = "Succese";
                    _popup.SetActive(true);
                    _popup.SetText(str);
                }
                else
                {
                    string str = "Fail";
                    _popup.SetActive(true);
                    _popup.SetText(str);
                }
            }
            _playerStat.DeletePlayerGold(NeedReinforGold);
            //_playerStat.Gold -= NeedReinforGold;
            AddReinforGold(TableID);
            SetNeedGold(TableID);
            _inventory.SetPlayerGold(_playerStat.Gold);
        }
        else
        {
            string str = "NO Gold";
            _popup.SetActive(true);
            _popup.SetText(str);
        }
        
    }

    public void AddReinforGold(int tableid)
    {
        int nextgold = NeedReinforGold + Item.ItemStatDic[tableid].AddReinforGold;
        Item.ItemStatDic[tableid].ReinforGold = nextgold;
        Item.ItemDataDic[tableid]["ReinforGold"] = nextgold.ToString();
    }

    public void Open()
    {
        canvas.sortingOrder = SetSortOrder();
        canvas.gameObject.SetActive(true);
        IsOpenPopup = true;
    }

    public void Exit()
    {
        if (!ReiforItem.IsEmepty())
        {
            string str = "NO";
            _popup.SetActive(true);
            _popup.SetText(str);
            return;
        }
        canvas.gameObject.SetActive(false);
        canvas.sortingOrder = ResetSortingOrder();
        WeaponShopUI.IsPopupOpen = false;
        IsOpenPopup = false;
    }

}
