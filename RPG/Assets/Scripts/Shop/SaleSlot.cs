using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SaleSlot : MonoBehaviour,IPointerClickHandler
{
    public Image ItemImage = null;
    public Text ItemSalePriceText = null;
    public Text ItemText = null;
    public Button Exitbutton;
    public Image ItemCountImage;
    public Text ItemCountText;

    public int SalePrice = 0;
    public int _tableId;
    public int _uniqueId;
    public bool IsDeActive;

    WeaponShopSalePopupUI weaponsalePopup;
    ItemShopSalePopupUI itemsalePopup;
    public void GetText(string SalePriceText, string Itemname, int tableId,int uniqueid)
    {
        ItemText.text = Itemname;
        ItemSalePriceText.text = SalePriceText;
        int.TryParse(SalePriceText, out SalePrice);
        _tableId = tableId;
        _uniqueId = uniqueid;
        SetItemCount(uniqueid);
        Exitbutton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        print("OnClick");
        GetSalePopup();
        IsDeActive = true;
        if (weaponsalePopup != null)
        {
            WeaponShopDeleteSaleSlot();
        }
        if(itemsalePopup != null)
        {
            ItemShopDeleteSaleSlot();
        }
    }

    public void SetItemCount(int UniqueId)
    {
        if(UniqueId <= 1)
        {
            ItemCountImage.gameObject.SetActive(false);
            ItemCountText.gameObject.SetActive(false);
        }
        if(UniqueId > 1)
        {
            ItemCountImage.gameObject.SetActive(true);
            ItemCountText.gameObject.SetActive(true);
            ItemCountText.text = UniqueId.ToString();
        }
    }

    public void GetSalePopup()
    {
        weaponsalePopup = transform.GetComponentInParent<WeaponShopSalePopupUI>();
        if(weaponsalePopup != null)
        {
            return;
        }
        itemsalePopup = transform.GetComponentInParent<ItemShopSalePopupUI>();
        if(itemsalePopup != null)
        {
            return;
        }
    }

    public void SetActive(bool state)
    {
        this.gameObject.SetActive(state);
    }

    public void WeaponShopDeleteSaleSlot()
    {
        weaponsalePopup.DeleteSaleSlot();
    }

    public void ItemShopDeleteSaleSlot()
    {
        itemsalePopup.DeleteSaleSlot();
    }

    public void GetSprite(Sprite sprite)
    {
        ItemImage.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        

    }
}
