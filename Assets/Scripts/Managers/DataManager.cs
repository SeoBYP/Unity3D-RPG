using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum TableType
{
    CharacterInformation,
    ItemInformation,
    ShopInformaition,
    SkillInfomation,
    InventorySlotInformation,
    PlayerStateInformation,
    QuestInformation,
}

public static class DataManager
{
    public static Dictionary<TableType, LowBase> TableDic = new Dictionary<TableType, LowBase>();
    public static void Load(TableType tableType)
    {
        string path = Application.persistentDataPath + "/Data/" + tableType.ToString();
        if (!TableDic.ContainsKey(tableType))
        {
            if (File.Exists(path))
            {
                LowBase saveBase = new LowBase();
                saveBase.LoadSaveData(path);
                TableDic.Add(tableType, saveBase);
                return;
            }

            LowBase lowBase = new LowBase();
            lowBase.LoadData("Data/" + tableType.ToString());
            TableDic.Add(tableType, lowBase);
        }
            
    }
    public static int ToInter(TableType tableType ,int tableIndex, string subject)
    {
        if (TableDic.ContainsKey(tableType))
            return TableDic[tableType].ToInter(tableIndex, subject);

        return 0;

    }

    public static float ToFloat(TableType tableType, int tableIndex, string subject)
    {
        if (TableDic.ContainsKey(tableType))
            return TableDic[tableType].Tofloat(tableIndex, subject);

        return 0;
    }

    public static string ToString(TableType tableType, int tableIndex, string subject)
    {
        if (TableDic.ContainsKey(tableType))
            return TableDic[tableType].Tostring(tableIndex, subject);

        return string.Empty;
    }

}
