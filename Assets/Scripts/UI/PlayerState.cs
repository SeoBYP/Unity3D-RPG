using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;
public class PlayerState : BaseUI
{
    Canvas canvas;
    Dictionary<int, StateItemSlot> StatusDic = new Dictionary<int, StateItemSlot>();

    public Slider HP_bar;
    public Slider MP_bar;
    public Slider EXP_bar;
    public TMP_Text HP;
    public TMP_Text MP;
    public TMP_Text EXP;
    public Button _StateExitBtn;
    public StatText statText;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        _StateExitBtn.onClick.AddListener(ExitBtn);
        StateItemSlot[] _itemSlot = GetComponentsInChildren<StateItemSlot>();
        if (_itemSlot != null)
        {
            for (int i = 0; i < _itemSlot.Length; i++)
            {
                _itemSlot[i].Init();
                StatusDic.Add(i, _itemSlot[i]);
                CheckAndSetType(_itemSlot[i].name, _itemSlot[i]);
                SetStateslot(i);
            }
        }
        HP.text = $"{_playerStat.HP} / {_playerStat.MaxHP}";
        MP.text = $"{_playerStat.MP} / {_playerStat.MaxMP}";
        EXP.text = $"{_playerStat.Exp}";
    }

    public void SetInStateHPBar(float hpvalue)
    {
        HP_bar.value = hpvalue;
        HP.text = $"{_playerStat.HP} / {_playerStat.MaxHP}";
    }

    public void SetInStateMPBar(float mpvalue)
    {
        MP_bar.value = mpvalue;
        MP.text = $"{_playerStat.MP} / {_playerStat.MaxMP}";
    }

    public void SetInStateEXPBar(float expvalue)
    {
        EXP_bar.value = expvalue;
        EXP.text = $"{_playerStat.Exp}";
    }

    public void SetStateslot(int id)
    {
        if (StatusDic.ContainsKey(id))
        {
            if (DataManager.TableDic.ContainsKey(TableType.PlayerStateInformation))
            {
                int tableid = DataManager.ToInter(TableType.PlayerStateInformation, id, "ItemTableID");
                int uniqueid = DataManager.ToInter(TableType.PlayerStateInformation, id, "UniqueID");
                string slotname = DataManager.ToString(TableType.PlayerStateInformation, id, "SlotName");

                StatusDic[id].SetInfo(tableid, uniqueid);
                StatusDic[id].SetPlayerStat(tableid);
            }
        }
    }

    public void CheckAndSetType(string name,StateItemSlot stateItemSlot)
    {
        switch (name)
        {
            case "Head":
                stateItemSlot.SetStateItemSlotType(ItemType.Head);
                break;
            case "LeftHand":
                stateItemSlot.SetStateItemSlotType(ItemType.Hand);
                break;
            case "RightHand":
                stateItemSlot.SetStateItemSlotType(ItemType.Weapon);
                break;
            case "UpperBody":
                stateItemSlot.SetStateItemSlotType(ItemType.UpperBody);
                break;
            case "LowerBody":
                stateItemSlot.SetStateItemSlotType(ItemType.LowerBody);
                break;
            case "Shoes":
                stateItemSlot.SetStateItemSlotType(ItemType.Shoes);
                break;
            case "Accessories1":
                stateItemSlot.SetStateItemSlotType(ItemType.Accessories);
                break;
            case "Accessories2":
                stateItemSlot.SetStateItemSlotType(ItemType.Accessories);
                break;
            case "Accessories3":
                stateItemSlot.SetStateItemSlotType(ItemType.Accessories);
                break;
        }
    }

    public void SetStatText()
    {
        if(statText != null)
        {
            statText.SetDefaultStat(_playerStat);
        }
    }

    public override void ExitBtn()
    {
        canvas.gameObject.SetActive(false);
        IsOpenPopup = false;
    }

    public void OpenAndClose()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
            canvas.sortingOrder = ResetSortingOrder();
            IsOpenPopup = false;
        }
        else
        {
            canvas.sortingOrder = SetSortOrder();
            canvas.gameObject.SetActive(true);
            IsOpenPopup = true;
        }
    }

    public override string ToString()
    {
        string text = string.Empty;
        text += "ID,SlotName,ItemType,ItemTableID,UniqueID\n";
        for(int i = 0; i < StatusDic.Count; i++)
        {
            string newid = i.ToString();
            string newslotname = StatusDic[i].name;
            string newItemType = StatusDic[i].SlotItemType.ToString();
            string newitemtableid = StatusDic[i].ItemTableId.ToString();
            string newuniqueid = StatusDic[i].UniqueId.ToString();

            text += $"{newid},{newslotname},{newItemType},{newitemtableid},{newuniqueid}\n";
        }
        return text;
    }

    public void SaveState(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffers = System.Text.Encoding.UTF8.GetBytes(ToString());
        fs.Write(buffers, 0, buffers.Length);
        fs.Close();
    }
}
