using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class QuestInfo
{
    private int _ID;
    private string _Name;
    private int _NeedLevel;
    private int _RewardItemTable;
    private int _RewardItemCount;
    private int _RewardGold;
    private int _RewardEXP;
    private bool _IsClear;
    private string _Info;
    private string _Condition;
    private int _CurrentConditionCount;
    private int _ConditionCount;
    private bool _IsPlayerHave;
    private bool _DidClear;

    public int ID { get { return _ID; } set { _ID = value; } }
    public string Name { get { return _Name; } set { _Name = value; } }
    public int NeedLevel { get { return _NeedLevel; } set { _NeedLevel = value; } }
    public int RewardItemTable { get { return _RewardItemTable; } set { _RewardItemTable = value; } }
    public int RewardItemCount { get { return _RewardItemCount; } set { _RewardItemCount = value; } }
    public int RewardGold { get { return _RewardGold; } set { _RewardGold = value; } }
    public int RewardEXP { get { return _RewardEXP; } set { _RewardEXP = value; } }
    public bool IsClear { get { return _IsClear; } set { _IsClear = value; } }
    public string Info { get { return _Info; } set { _Info = value; } }
    public string Condition { get { return _Condition; } set { _Condition = value; } }
    public int CurrentConditionCount { get { return _CurrentConditionCount; } set { _CurrentConditionCount = value; } }
    public int ConditionCount { get { return _ConditionCount; } set { _ConditionCount = value; } }
    public bool IsPlayerHave { get { return _IsPlayerHave; } set { _IsPlayerHave = value; } }
    public bool DidClear { get { return _DidClear; } set { _DidClear = value; } }

    public void SetQuestInfo(int tableid)
    {
        _ID = tableid;
        _Name = DataManager.ToString(TableType.QuestInformation, tableid, "Name");
        _NeedLevel = DataManager.ToInter(TableType.QuestInformation, tableid, "NeedLevel");
        _RewardItemTable = DataManager.ToInter(TableType.QuestInformation, tableid, "RewardItemTable");
        _RewardItemCount = DataManager.ToInter(TableType.QuestInformation, tableid, "RewardItemCount");
        _RewardGold = DataManager.ToInter(TableType.QuestInformation, tableid, "RewardGold");
        _RewardEXP = DataManager.ToInter(TableType.QuestInformation, tableid, "RewardEXP");
        _IsClear = SetBoolClear(tableid);
        _Info = DataManager.ToString(TableType.QuestInformation, tableid, "Info");
        _Condition = DataManager.ToString(TableType.QuestInformation, tableid, "Condition");
        _CurrentConditionCount = DataManager.ToInter(TableType.QuestInformation, tableid, "CurrentConditionCount");
        _ConditionCount = DataManager.ToInter(TableType.QuestInformation, tableid, "ConditionCount");
        _IsPlayerHave = SetBoolIsHavePlayer(tableid);
        _DidClear = SetDidClear(tableid);
    }
    public bool SetBoolClear(int tableid)
    {
        string isclear = DataManager.ToString(TableType.QuestInformation, tableid, "IsClear");
        if (isclear == "False")
        {
            return false;
        }
        return true;
    }

    public bool SetBoolIsHavePlayer(int tableid)
    {
        string isclear = DataManager.ToString(TableType.QuestInformation, tableid, "IsPlayerHave");
        if (isclear == "False")
        {
            return false;
        }
        return true;
    }

    public bool SetDidClear(int tableid)
    {
        string isclear = DataManager.ToString(TableType.QuestInformation, tableid, "DidClear");
        if (isclear == "False")
        {
            return false;
        }
        return true;
    }

    public override string ToString()
    {
        string text = string.Empty;
        text += $"{_ID},{_Name},{_NeedLevel},{_RewardItemTable},{_RewardItemCount},{_RewardGold},{_RewardEXP},{_IsClear},{_Info},{_Condition},{_CurrentConditionCount},{_ConditionCount},{_IsPlayerHave},{_DidClear}\n";
        return text;
    }
}

public static class Quest
{
    public static Dictionary<int, QuestInfo> QuestInfoDic = new Dictionary<int, QuestInfo>();

    public static void SetQuestInfo()
    {
        if (DataManager.TableDic.ContainsKey(TableType.QuestInformation))
        {
            for(int i = 1; i <= DataManager.TableDic[TableType.QuestInformation].InfoDic.Count; i++)
            {
                if (!QuestInfoDic.ContainsKey(i))
                {
                    QuestInfoDic.Add(i,new QuestInfo());
                }
                if (QuestInfoDic.ContainsKey(i))
                {
                    QuestInfoDic[i].SetQuestInfo(i);
                }
            }
        }
    }
    public static void ClearQuest(int tableid)
    {
        if (QuestInfoDic.ContainsKey(tableid))
        {
            bool isclear = QuestInfoDic[tableid].IsClear;
            if (!isclear)
            {
                QuestInfoDic[tableid].IsClear = true;
            }
        }
    }

    public static string QuestDataByte()
    {
        string text = string.Empty;
        text += "ID,Name,NeedLevel,RewardItemTable,RewardItemCount,RewardGold,RewardEXP,IsClear,Info,Condition,CurrentConditionCount,ConditionCount,IsPlayerHave,DidClear\n";
        for (int i = 0; i <= QuestInfoDic.Count; i++)
        {
            if (QuestInfoDic.ContainsKey(i))
            {
                text += QuestInfoDic[i].ToString();
            }
        }
        return text;
    }

    public static void CheckCondition(string name,int questid)
    {
        if (QuestInfoDic.ContainsKey(questid))
        {
            string condition = QuestInfoDic[questid].Condition;
            if(name == condition)
            {
                QuestInfoDic[questid].CurrentConditionCount += 1;
                int currentconditioncount = QuestInfoDic[questid].CurrentConditionCount;
                int conditioncount = QuestInfoDic[questid].ConditionCount;
                if(currentconditioncount >= conditioncount)
                {
                    QuestInfoDic[questid].IsClear = true;
                }
                Debug.Log(QuestInfoDic[questid].IsClear);
            }
        }
    }

    public static void SaveQuestData(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffers = System.Text.Encoding.UTF8.GetBytes(QuestDataByte());
        fs.Write(buffers, 0, buffers.Length);
        fs.Close();
    }
}
