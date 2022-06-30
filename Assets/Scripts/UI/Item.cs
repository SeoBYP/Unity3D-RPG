using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ItemStat
{
    private int _ID;
    private string _Name;
    private int _Level;
    private int _Attack;
    private int _Critical;
    private int _Defence;
    private int _Probability;
    private int _MaxLevel;
    private int _Hp;
    private int _Mp;
    private ItemType _Type;
    private int _AddAttack;
    private int _AddCritical;
    private int _AddDefence;
    private int _ReinforGold;
    private int _AddReinforGold;
    private float _CoolTime;

    public int ID { get { return _ID; } set { _ID = value; } }
    public string Name { get { return _Name; } set { _Name = value; } }
    public int Level { get { return _Level; } set { _Level = value; } }
    public int Attack { get { return _Attack; } set { _Attack = value; } }
    public int Critical { get { return _Critical; } set { _Critical = value; } }
    public int Defence { get { return _Defence; } set { _Defence = value; } }
    public int Probability { get { return _Probability; } set { _Probability = value; } }
    public int MaxLevel { get { return _MaxLevel; } set { MaxLevel = value; } }
    public int HP { get { return _Hp; } set { _Hp = value; } }
    public int MP { get { return _Mp; } set { _Mp = value; } }
    public ItemType Type { get { return _Type; } set { _Type = value; } }
    public int AddAttack { get { return _AddAttack; } set { _AddAttack = value; } }
    public int AddCritical { get { return _AddCritical; } set { _AddCritical = value; } }
    public int AddDefence { get { return _AddDefence; } set { _AddDefence = value; } }
    public int ReinforGold { get { return _ReinforGold; } set { _ReinforGold = value; } }
    public int AddReinforGold { get { return _AddReinforGold; } set { _AddReinforGold = value; } }
    public float CoolTime { get { return _CoolTime; } set { _CoolTime = value; } }

    public void SetItemStat(int TableID)
    {
        _ID = TableID;//DataManager.ToInter(TableType.ItemInformation, TableID, "ID");
        _Name = DataManager.ToString(TableType.ItemInformation, TableID, "Name");
        _Level = DataManager.ToInter(TableType.ItemInformation, TableID, "Level");
        _Attack = DataManager.ToInter(TableType.ItemInformation, TableID, "Attack");
        _Critical = DataManager.ToInter(TableType.ItemInformation, TableID, "Critical");
        _Defence = DataManager.ToInter(TableType.ItemInformation, TableID, "Defence");
        _Probability = DataManager.ToInter(TableType.ItemInformation, TableID, "Probability");
        _MaxLevel = DataManager.ToInter(TableType.ItemInformation, TableID, "MaxLevel");
        _Hp = DataManager.ToInter(TableType.ItemInformation, TableID, "Hp");
        _Mp = DataManager.ToInter(TableType.ItemInformation, TableID, "Mp");
        _Type = SetItemType(DataManager.ToString(TableType.ItemInformation, TableID, "Type"));
        _AddAttack = DataManager.ToInter(TableType.ItemInformation, TableID, "AddAttack");
        _AddCritical = DataManager.ToInter(TableType.ItemInformation, TableID, "AddCritical");
        _AddDefence = DataManager.ToInter(TableType.ItemInformation, TableID, "AddDefence");
        _ReinforGold = DataManager.ToInter(TableType.ItemInformation, TableID, "ReinforGold");
        _AddReinforGold = DataManager.ToInter(TableType.ItemInformation, TableID, "AddReinforGold");
        _CoolTime = DataManager.ToFloat(TableType.ItemInformation, TableID, "CoolTime");
    }

    public override string ToString()
    {
        string text = string.Empty;
        text += $"{_ID},{_Name},{_Level},{_Attack},{_Critical},{_Defence},{_Probability},{_MaxLevel},{_Hp},{_Mp},{_Type},{_AddAttack},{_AddCritical},{_AddDefence},{_ReinforGold},{_AddReinforGold},{_CoolTime}\n";
        return text;
    }

    public ItemType SetItemType(string str)
    {
        switch (str)
        {
            case "Weapon":
                return ItemType.Weapon;
            case "UpperBody":
                return ItemType.UpperBody;
            case "LowerBody":
                return ItemType.LowerBody;
            case "Shoes":
                return ItemType.Shoes;
            case "Accessories":
                return ItemType.Accessories;
            case "Hand":
                return ItemType.Hand;
            case "Item":
                return ItemType.Item;
            case "Head":
                return ItemType.Head;
        }
        return ItemType.Null;
    }
}

public static class Item
{
    //public static Dictionary<int, Dictionary<Sprite, Dictionary<string, string>>> ItemDic = new Dictionary<int, Dictionary<Sprite, Dictionary<string, string>>>();
    public static Dictionary<int, Dictionary<string, string>> ItemDataDic = new Dictionary<int, Dictionary<string, string>>();
    public static Dictionary<int, Sprite> ItemIconDIc = new Dictionary<int, Sprite>();
    public static Dictionary<int, ItemStat> ItemStatDic = new Dictionary<int, ItemStat>();

    public static void ItemData()
    {
        if (DataManager.TableDic.ContainsKey(TableType.ItemInformation))
        {
            for(int i = 1; i<= DataManager.TableDic[TableType.ItemInformation].InfoDic.Count; i++)
            {
                if (!ItemDataDic.ContainsKey(i))
                {
                    ItemDataDic.Add(i, new Dictionary<string, string>());
                }
                //ItemDataDic[i].Add("ID", DataManager.ToString(TableType.ItemInformation, i, "ID"));
                if (ItemDataDic.ContainsKey(i))
                {
                    ItemDataDic[i].Add("Name", DataManager.ToString(TableType.ItemInformation, i, "Name"));
                    ItemDataDic[i].Add("Level", DataManager.ToString(TableType.ItemInformation, i, "Level"));
                    ItemDataDic[i].Add("Attack", DataManager.ToString(TableType.ItemInformation, i, "Attack"));
                    ItemDataDic[i].Add("Critical", DataManager.ToString(TableType.ItemInformation, i, "Critical"));
                    ItemDataDic[i].Add("Defence", DataManager.ToString(TableType.ItemInformation, i, "Defence"));
                    ItemDataDic[i].Add("Probability", DataManager.ToString(TableType.ItemInformation, i, "Probability"));
                    ItemDataDic[i].Add("MaxLevel", DataManager.ToString(TableType.ItemInformation, i, "MaxLevel"));
                    ItemDataDic[i].Add("Hp", DataManager.ToString(TableType.ItemInformation, i, "Hp"));
                    ItemDataDic[i].Add("Mp", DataManager.ToString(TableType.ItemInformation, i, "Mp"));
                    ItemDataDic[i].Add("Type", DataManager.ToString(TableType.ItemInformation, i, "Type"));
                    ItemDataDic[i].Add("AddAttack", DataManager.ToString(TableType.ItemInformation, i, "AddAttack"));
                    ItemDataDic[i].Add("AddCritical", DataManager.ToString(TableType.ItemInformation, i, "AddCritical"));
                    ItemDataDic[i].Add("AddDefence", DataManager.ToString(TableType.ItemInformation, i, "AddDefence"));
                    ItemDataDic[i].Add("ReinforGold", DataManager.ToString(TableType.ItemInformation, i, "ReinforGold"));
                    ItemDataDic[i].Add("AddReinforGold", DataManager.ToString(TableType.ItemInformation, i, "AddReinforGold"));
                    ItemDataDic[i].Add("CoolTime", DataManager.ToString(TableType.ItemInformation, i, "CoolTime"));
                    ItemDataDic[i].Add("Price", DataManager.ToString(TableType.ShopInformaition, i, "Price"));
                    ItemDataDic[i].Add("SalePrice", DataManager.ToString(TableType.ShopInformaition, i, "SalePrice"));
                }
                if (!ItemStatDic.ContainsKey(i))
                {
                    ItemStatDic.Add(i, new ItemStat());
                    ItemStatDic[i].SetItemStat(i);
                }
            }
        }

        for (int i = 1; i <= ItemDataDic.Count; i++)
        {
            if (ItemDataDic.ContainsKey(i))
            {
                Sprite Icon = Resources.Load<Sprite>("Items/" + ItemDataDic[i]["Name"]);
                ItemIconDIc.Add(i, Icon);
            }
        }

    }

    public static void AddItemStat(int itemtableid)
    {
        int CurrentLevel = ItemStatDic[itemtableid].Level;
        if(CurrentLevel < ItemStatDic[itemtableid].MaxLevel)
        {
            ItemStatDic[itemtableid].Level += 1;
            ItemStatDic[itemtableid].Attack += ItemStatDic[itemtableid].AddAttack;
            ItemStatDic[itemtableid].Critical += ItemStatDic[itemtableid].AddCritical;
            ItemStatDic[itemtableid].Defence += ItemStatDic[itemtableid].AddDefence;
            ItemStatDic[itemtableid].Probability -= 3;

            int newLevel = ItemStatDic[itemtableid].Level;
            int newAttackStat = ItemStatDic[itemtableid].Attack;
            int newCriticalStat = ItemStatDic[itemtableid].Critical;
            int newDefenceStat = ItemStatDic[itemtableid].Defence;
            int newProbability = ItemStatDic[itemtableid].Probability;

            ItemDataDic[itemtableid]["Level"] = newLevel.ToString();
            ItemDataDic[itemtableid]["Attack"] = newAttackStat.ToString();
            ItemDataDic[itemtableid]["Critical"] = newCriticalStat.ToString();
            ItemDataDic[itemtableid]["Defence"] = newDefenceStat.ToString();
            ItemDataDic[itemtableid]["Probability"] = newProbability.ToString();
        }
    }

    public static string ShopData()
    {
        string text = string.Empty;
        text += "ID,Name,Price,SalePrice\n";
        for (int i = 0; i <= ItemDataDic.Count; i++)
        {
            if (ItemDataDic.ContainsKey(i))
            {
                string newid = i.ToString();
                string newname = ItemDataDic[i]["Name"];
                string newprice = ItemDataDic[i]["Price"];
                string newsalepeice = ItemDataDic[i]["SalePrice"];
                text += $"{newid},{newname},{newprice},{newsalepeice}\n";
            }
        }
        return text;
    }

    public static string ItemDataByte()
    {
        string text = string.Empty;
        text += "ID,Name,Level,Attack,Critical,Defence,Probability,MaxLevel,Hp,Mp,Type,AddAttack,AddCritical,AddDefence,ReinforGold,AddReinforGold,CoolTime\n";
        for (int i = 0; i <= ItemStatDic.Count; i++)
        {
            if (ItemStatDic.ContainsKey(i))
            {
                text += ItemStatDic[i].ToString();
            }
        }
        return text;
    }

    public static void SaveItemData(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffers = System.Text.Encoding.UTF8.GetBytes(ItemDataByte());
        fs.Write(buffers, 0, buffers.Length);
        fs.Close();
    }

    public static void SaveShopData(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffers = System.Text.Encoding.UTF8.GetBytes(ShopData());
        fs.Write(buffers, 0, buffers.Length);
        fs.Close();
    }

}
