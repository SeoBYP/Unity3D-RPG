using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    private Animator NPCAni;
    private BoxCollider _collider;
    ItemShopUI _ItemShopUI;

    public static bool _openWeaponShop = false;
    public void Start()
    {
        NPCAni = GetComponentInChildren<Animator>();
        _collider = GetComponent<BoxCollider>();
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (InputManager.CommunityActive)
            {
                _ItemShopUI = UIManager.Instance.Get<ItemShopUI>(UIList.ItemShopUI);
                _openWeaponShop = true;
                OpenItemShop();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (ItemShopUI.IsItemPopupOpen)
            {
                _ItemShopUI.Exit();
                ItemShopUI.IsItemPopupOpen = false;
            }
        }
    }

    public void OpenItemShop()
    {
        _ItemShopUI.OpenAndClose();
    }
}
