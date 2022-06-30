using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponShopSalePopupUI : BaseUI,IDropHandler
{
    Canvas canvas;
    public Button ExitButton;
    public Text SalePriceText;
    public Button SaleButton;
    Inventory _inventory;
    WeaponShopContents contents;
    DragAndDropContainer _dragContainer;
    SaleSlot saleSlot;

    private int itemTableID;
    private int itemUniqueID;
    private int TotalPrice = 0;

    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        ExitButton.onClick.AddListener(Exit);
        SaleButton.onClick.AddListener(Sale);
        contents = GetComponentInChildren<WeaponShopContents>();
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        _dragContainer = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
        saleSlot = Resources.Load<SaleSlot>("Prefabs/UI/SaleSlot");
        SalePriceText.text = TotalPrice.ToString();
    }

    public void Open()
    {
        canvas.gameObject.SetActive(true);
        canvas.sortingOrder = SetSortOrder();
        _inventory.OpenAndClose();
        IsOpenPopup = true;
    }

    void Exit()
    {
        SaleSlot saleslot = contents.transform.GetComponentInChildren<SaleSlot>();
        if(saleslot != null)
        {
            _popup.SetActive(true);
            string str = "It Has Item";
            _popup.SetText(str);
            return;
        }
        transform.gameObject.SetActive(false);
        canvas.sortingOrder = ResetSortingOrder();
        WeaponShopUI.IsPopupOpen = false;
        _inventory.OpenAndClose();
        IsOpenPopup = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (_dragContainer.itemslot == null)
            return;

        if(_dragContainer.itemslot != null)
        {
            SaleSlot newsaleslot = Instantiate<SaleSlot>(saleSlot, contents.transform);
            itemTableID = _dragContainer.itemslot.ItemTableId;
            itemUniqueID = _dragContainer.itemslot.UniqueId;
            Sprite sprite = _dragContainer.itemslot.Icon;
            string SalePrice = Item.ItemDataDic[itemTableID]["SalePrice"];
            string Name = Item.ItemDataDic[itemTableID]["Name"];

            newsaleslot.GetSprite(sprite);
            newsaleslot.GetText(SalePrice, Name, itemTableID, itemUniqueID);
            _dragContainer.itemslot.SetEmptyIcon();
            _dragContainer.itemslot.SetItemCountImgae(false);
            _dragContainer.itemslot = null;
            SalePriceText.text = PriceText(SalePrice, itemUniqueID);
        }
    }

    public void ReSetSaleSlot()
    {
        
    }

    private string PriceText(string price,int uniqueId)
    {
        int m_price = 0;
        int.TryParse(price, out m_price);
        if(uniqueId > 1)
            m_price *= uniqueId;
        TotalPrice += m_price;
        return TotalPrice.ToString();
    }

    private string DeletePrice(string price, int uniqueId)
    {
        int m_price = 0;
        print(uniqueId);
        int.TryParse(price, out m_price);
        if (uniqueId > 1)
            m_price *= uniqueId;
        print(m_price);
        TotalPrice -= m_price;
        return TotalPrice.ToString();
    }

    public void DeleteSaleSlot()
    {
        SaleSlot[] saleSlots = contents.transform.GetComponentsInChildren<SaleSlot>();
        for(int i = 0; i < saleSlots.Length; i++)
        {
            if (saleSlots[i].IsDeActive)
            {
                Sprite sprite = saleSlots[i].ItemImage.sprite;
                int thistableid = saleSlots[i]._tableId;
                int thisuniqueid = saleSlots[i]._uniqueId;

                FoundInventoryEmptySlot(sprite,thistableid,thisuniqueid);

                Destroy(saleSlots[i].gameObject);
            }
        }
    }

    public void FoundInventoryEmptySlot(Sprite sprite, int tableId,int unique)
    {
        for (int i = 0; i < _inventory.InventorySlot.Count; i++)
        {
            if (_inventory.InventorySlot[i].ItemTableId == tableId)
            {
                _inventory.InventorySlot[i].SetInfo(tableId, unique);
                string SalePrice = Item.ItemDataDic[tableId]["SalePrice"];
                SalePriceText.text = DeletePrice(SalePrice, unique);
                return;
            }
            if (_inventory.InventorySlot[i].IsEmepty())
            {
                _inventory.InventorySlot[i].SetInfo(tableId, unique);
                string SalePrice = Item.ItemDataDic[tableId]["SalePrice"];
                SalePriceText.text = DeletePrice(SalePrice, unique);
                return;
            }
        }
        
    }

    public void Sale()
    {
        _playerStat.Gold += TotalPrice;
        _inventory.SetPlayerGold(_playerStat.Gold);
        
        TotalPrice = 0;
        SalePriceText.text = TotalPrice.ToString();
        SaleSlot[] saleSlots = contents.transform.GetComponentsInChildren<SaleSlot>();
        for (int i = 0; i < saleSlots.Length; i++)
        {
            Destroy(saleSlots[i].gameObject);
        }
    }

}
