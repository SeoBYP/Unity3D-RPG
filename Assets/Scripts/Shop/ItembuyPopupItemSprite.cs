using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItembuyPopupItemSprite : ItemIcon
{
    Image image;
    int itemtableid;
    public Transform ItemInfoPos;

    public void SetTableID(int tableid)
    {
        image = GetComponent<Image>();
        itemtableid = tableid;
        if(Item.ItemIconDIc.ContainsKey(itemtableid))
            image.sprite = Item.ItemIconDIc[itemtableid];
    }

    public override void ShowItemInfo()
    {
        if (itemtableid == 0)
            return;
        ShowItemInfo itemInfo = UIManager.Instance.Get<ShowItemInfo>(UIList.ShowItemInfo);
        itemInfo.ShowInfo(ItemInfoPos.position, itemtableid, true);
    }

    public override void DeactiveItemInfo()
    {
        ShowItemInfo itemInfo = UIManager.Instance.Get<ShowItemInfo>(UIList.ShowItemInfo);
        itemInfo.ShowInfo(ItemInfoPos.position, itemtableid, false);
    }
}
