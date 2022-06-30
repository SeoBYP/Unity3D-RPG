using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItembuyPopup : BaseUI
{
    Canvas canvas;
    Inventory _inventory;
    public ItembuyPopupItemSprite BuyItemImage;
    public Text PriceText;
    public Text ItemCountText;
    public Button _BuyBtn;
    public Button _ExitBtn;
    public Button PlusCount;
    public Button MinusCount;
    public int SlotItemtableID;
    private int _ItemCount;
    private int _itemPrice;
    private int currPrice;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        _inventory = UIManager.Instance.Get<Inventory>(UIList.Inventory);
        BuyItemImage = GetComponentInChildren<ItembuyPopupItemSprite>();
        if (_popup == null)
        {
            print("_popupUI is Null");
        }
        PlusCount.onClick.AddListener(OnPlusBtn);
        MinusCount.onClick.AddListener(OnMinusBtn);
        _ExitBtn.onClick.AddListener(OnExit);
        _BuyBtn.onClick.AddListener(Buy);
    }
    public void Setting(Sprite sprite,string str,int tableId)
    {
        canvas.gameObject.SetActive(true);
        canvas.sortingOrder = SetSortOrder();
        _itemPrice = int.Parse(str);
        currPrice = _itemPrice;
        SlotItemtableID = tableId;
        BuyItemImage.SetTableID(SlotItemtableID);
        _ItemCount = 1;
        PriceText.text = _itemPrice.ToString();
        ItemCountText.text = _ItemCount.ToString();

    }

    public void OnPlusBtn()
    {
        _ItemCount++;
        _itemPrice += currPrice;
        PriceText.text = _itemPrice.ToString();
        ItemCountText.text = _ItemCount.ToString();
    }

    public void OnMinusBtn()
    {
        if(_ItemCount > 1)
        {
            _ItemCount--;
            _itemPrice -= currPrice;
            PriceText.text = _itemPrice.ToString();
            ItemCountText.text = _ItemCount.ToString();
        }
    }

    public void OnExit()
    {
        canvas.gameObject.SetActive(false);
    }

    public void Buy()
    {
        if (_itemPrice <= _playerStat.Gold)
        {
            for(int j = 0; j < _ItemCount; j++)
            {
                for (int i = 0; i < _inventory.InventorySlot.Count; i++)
                {
                    if(_inventory.InventorySlot[i].ItemTableId == SlotItemtableID)
                    {
                        _inventory.InventorySlot[i].UniqueId += _ItemCount;
                        int newunique = _inventory.InventorySlot[i].UniqueId;
                        _inventory.InventorySlot[i].SetInfo(SlotItemtableID, newunique);
                        _playerStat.DeletePlayerGold(_itemPrice);
                        //_playerStat.Gold -= _itemPrice;
                        _inventory.SetPlayerGold(_playerStat.Gold);
                        return;
                    }
                    if (_inventory.InventorySlot[i].IsEmepty())
                    {
                        _inventory.InventorySlot[i].SetInfo(SlotItemtableID,_ItemCount);
                        _playerStat.DeletePlayerGold(_itemPrice);
                        //_playerStat.Gold -= _itemPrice;
                        _inventory.SetPlayerGold(_playerStat.Gold);
                        return;
                    }
                }
            }
            
        }
        else
        {
            string str = "??? ?????.";
            _popup.gameObject.SetActive(true);
            _popup.SetText(str);
        }
            
    }

    

}
