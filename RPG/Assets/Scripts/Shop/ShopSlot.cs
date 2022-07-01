using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour,IPointerClickHandler
{
    public ItembuyPopup itembuyPopup;
    public Image ItemImage = null;
    public Text ItemPriceText = null;
    public Text ItemText = null;
    public int _tableId;

    public void Init()
    {
        itembuyPopup = UIManager.Instance.Get<ItembuyPopup>(UIList.ItembuyPopup);
    }

    public void GetText(string PriceText,string Itemname,int tableId)
    {
        ItemText.text = Itemname;
        ItemPriceText.text = PriceText;
        _tableId = tableId;
    }

    public void GetSprite(Sprite sprite)
    {
        ItemImage.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //itembuyPopup = UIManager.Instance.Get<ItembuyPopup>(UIList.ItembuyPopup);
        if (itembuyPopup != null)
        {
            itembuyPopup.Setting(ItemImage.sprite,ItemPriceText.text,_tableId);
        }
        
    }
}
