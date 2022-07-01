using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaPonShop : MonoBehaviour
{
    private Animator NPCAni;
    private BoxCollider _collider;
    WeaponShopUI _weaPonShopUI;

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
                _weaPonShopUI = UIManager.Instance.Get<WeaponShopUI>(UIList.WeaponShopUI);
                _openWeaponShop = true;
                OpenWeaponShop();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (WeaponShopUI.IsPopupOpen)
        {
            _weaPonShopUI.gameObject.SetActive(false);
            WeaponShopUI.IsPopupOpen = false;
        }
    }

    public void OpenWeaponShop()
    {
        _weaPonShopUI.OpenAndClose();
    }
}
