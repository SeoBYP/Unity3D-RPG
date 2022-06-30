using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public enum SkillType
{
    Strengthen,
    Active,
    Null,
}
public class SkillStat
{
    private int _ID;
    private string _SkillName;
    private int _SkillAttack;
    private int _SkillCritical;
    private int _SkillAttackPercent;
    private int _AddSkillAttack;
    private int _AddSkillCritical;
    private int _AddSkillAttackPercent;
    private SkillType _Type;
    private int _SkillTime;
    private float _AttackPercent;
    private float _DefencePercent;
    private float _SpeedPercent;
    private float _AddTime;
    private float _AddAtkPer;
    private float _AddDefPer;
    private float _AddSpePer;
    private float _NeedMP;
    private float _DeleteMP;
    private int _Level;
    private int _MaxLevel;
    private int _NeedPlayerLevel;
    private int _CoolTime;
    private int _DeleteCoolTime;

    public int ID { get { return _ID; } set { _ID = value; } }
    public string SkillName { get { return _SkillName; } set { _SkillName = value; } }
    public int SkillAttack { get { return _SkillAttack; } set { _SkillAttack = value; } }
    public int SkillCritical { get { return _SkillCritical; } set { _SkillCritical = value; } }
    public int SkillAttackPercent { get { return _SkillAttackPercent; } set { _SkillAttackPercent = value; } }
    public int AddSkillAttack { get { return _AddSkillAttack; } set { _AddSkillAttack = value; } }
    public int AddSkillCritical { get { return _AddSkillCritical; } set { _AddSkillCritical = value; } }
    public int AddSkillAttackPercent { get { return _AddSkillAttackPercent; } set { _AddSkillAttackPercent = value; } }
    public SkillType Type { get { return _Type; } set { _Type = value; } }
    public int SkillTime { get { return _SkillTime; } set { _SkillTime = value; } }
    public float AttackPercent { get { return _AttackPercent; } set { _AttackPercent = value; } }
    public float DefencePercent { get { return _DefencePercent; } set { _DefencePercent = value; } }
    public float SpeedPercent { get { return _SpeedPercent; } set { _SpeedPercent = value; } }
    public float AddTime { get { return _AddTime; } set { _AddTime = value; } }
    public float AddAtkPer { get { return _AddAtkPer; } set { _AddAtkPer = value; } }
    public float AddDefPer { get { return _AddDefPer; } set { _AddDefPer = value; } }
    public float AddSpePer { get { return _AddSpePer; } set { _AddSpePer = value; } }
    public float NeedMP { get { return _NeedMP; } set { _NeedMP = value; } }
    public float DeleteMP { get { return _DeleteMP; } set { _DeleteMP = value; } }
    public int Level { get { return _Level; } set { _Level = value; } }
    public int MaxLevel { get { return _MaxLevel; } set { _MaxLevel = value; } }
    public int NeedPlayerLevel { get { return _NeedPlayerLevel; } }
    public int CoolTime { get { return _CoolTime; } set { _CoolTime = value; } }
    public int DeleteCoolTime { get { return _DeleteCoolTime; } set { _DeleteCoolTime = value; } }

    public void SetSkillStat(int tableid, TableType type)
    {
        _ID = tableid;
        _SkillName = DataManager.ToString(type, tableid, "SkillName");
        _SkillAttack = DataManager.ToInter(type, tableid, "SkillAttack");
        _SkillCritical = DataManager.ToInter(type, tableid, "SkillCritical");
        _SkillAttackPercent = DataManager.ToInter(type, tableid, "SkillAttackPercent");
        _AddSkillAttack = DataManager.ToInter(type, tableid, "AddSkillAttack");
        _AddSkillCritical = DataManager.ToInter(type, tableid, "AddSkillCritical");
        _AddSkillAttackPercent = DataManager.ToInter(type, tableid, "AddSkillAttackPercent");
        _Type = SetSkillType(DataManager.ToString(type, tableid, "Type"));
        _SkillTime = DataManager.ToInter(type, tableid, "SkillTime");
        _AttackPercent = DataManager.ToFloat(type, tableid, "AttackPercent");
        _DefencePercent = DataManager.ToFloat(type, tableid, "DefencePercent");
        _SpeedPercent = DataManager.ToFloat(type, tableid, "SpeedPercent");
        _AddTime = DataManager.ToFloat(type, tableid, "AddTime");
        _AddAtkPer = DataManager.ToFloat(type, tableid, "AddAtkPer");
        _AddDefPer = DataManager.ToFloat(type, tableid, "AddDefPer");
        _AddSpePer = DataManager.ToFloat(type, tableid, "AddSpePer");
        _NeedMP = DataManager.ToFloat(type, tableid, "NeedMP");
        _DeleteMP = DataManager.ToFloat(type, tableid, "DeleteMP");
        _Level = DataManager.ToInter(type, tableid, "Level");
        _MaxLevel = DataManager.ToInter(type, tableid, "MaxLevel");
        _NeedPlayerLevel = DataManager.ToInter(type, tableid, "NeedPlayerLevel");
        _CoolTime = DataManager.ToInter(type, tableid, "CoolTime");
        _DeleteCoolTime = DataManager.ToInter(type, tableid, "DeleteCoolTime");
    }

    public override string ToString()
    {
        string text = string.Empty;
        text += $"{_ID},{_SkillName},{_SkillAttack},{_SkillCritical},{_SkillAttackPercent},{_AddSkillAttack},{_AddSkillCritical},{_AddSkillAttackPercent}" +
            $",{_Type},{_SkillTime},{_AttackPercent},{_DefencePercent},{_SpeedPercent},{_AddTime},{_AddAtkPer},{_AddDefPer},{_AddSpePer},{_NeedMP},{_DeleteMP},{_Level},{_MaxLevel},{_NeedPlayerLevel},{_CoolTime}\n";
        
        return text;
    }

    public SkillType SetSkillType(string str)
    {
        switch (str)
        {
            case "Strengthen":
                return SkillType.Strengthen;
            case "Active":
                return SkillType.Active;
        }
        return SkillType.Null;
    }
}

public static class PlayerSkill
{
    public static Dictionary<int, SkillStat> PlayerSkillStatDic = new Dictionary<int, SkillStat>();
    public static Dictionary<int, Sprite> SkillIconDic = new Dictionary<int, Sprite>();

    public static void SkillData()
    {
        if (DataManager.TableDic.ContainsKey(TableType.SkillInfomation))
        {
            for(int i = 1; i <= DataManager.TableDic[TableType.SkillInfomation].InfoDic.Count; i++)
            {
                if (!PlayerSkillStatDic.ContainsKey(i))
                {
                    PlayerSkillStatDic.Add(i, new SkillStat());
                    PlayerSkillStatDic[i].SetSkillStat(i, TableType.SkillInfomation);
                }
            }
        }
        for (int i = 1; i <= PlayerSkillStatDic.Count; i++)
        {
            if (PlayerSkillStatDic.ContainsKey(i))
            {
                Sprite Icon = Resources.Load<Sprite>("PlayerSkill/" + PlayerSkillStatDic[i].SkillName);
                SkillIconDic.Add(i, Icon);
            }
        }
    }

    public static void SkillLevelUP(int tableId)
    {
        SkillType type = PlayerSkillStatDic[tableId].Type;
        switch (type)
        {
            case SkillType.Strengthen:
                StrengthenLevelUp(tableId);
                break;
            case SkillType.Active:
                ActiveLevelUp(tableId);
                break;
        }
    }

    public static void StrengthenLevelUp(int Skilltableid)
    {
        int SkillCurLevel = PlayerSkillStatDic[Skilltableid].Level;
        int SkillMaxLevel = PlayerSkillStatDic[Skilltableid].MaxLevel;
        int SkillAddStack = (int)(PlayerSkillStatDic[Skilltableid].AddTime);
        float SKillAddAttack = PlayerSkillStatDic[Skilltableid].AddAtkPer;
        float SkillAddDefence = PlayerSkillStatDic[Skilltableid].AddDefPer;
        float SkillAddSpeed = PlayerSkillStatDic[Skilltableid].AddSpePer;
        float SkillDeleteMp = PlayerSkillStatDic[Skilltableid].DeleteMP;
        int SkillDeleteCoolTime = PlayerSkillStatDic[Skilltableid].DeleteCoolTime;
        if(SkillCurLevel <= SkillMaxLevel)
        {
            PlayerSkillStatDic[Skilltableid].Level += 1;
            PlayerSkillStatDic[Skilltableid].SkillTime += SkillAddStack;
            PlayerSkillStatDic[Skilltableid].AttackPercent += SKillAddAttack;
            PlayerSkillStatDic[Skilltableid].DefencePercent += SkillAddDefence;
            PlayerSkillStatDic[Skilltableid].SpeedPercent += SkillAddSpeed;
            PlayerSkillStatDic[Skilltableid].NeedMP -= SkillDeleteMp;
            PlayerSkillStatDic[Skilltableid].CoolTime -= SkillDeleteCoolTime;
        }
    }

    public static void ActiveLevelUp(int skilltableid)
    {
        int SkillCurLevel = PlayerSkillStatDic[skilltableid].Level;
        int SkillMaxLevel = PlayerSkillStatDic[skilltableid].MaxLevel;
        int AddSkillAtk = PlayerSkillStatDic[skilltableid].AddSkillAttack;
        int AddSkillAtkper = PlayerSkillStatDic[skilltableid].AddSkillAttackPercent;
        int AddSkillCri = PlayerSkillStatDic[skilltableid].AddSkillCritical;
        float SkillDeleteMp = PlayerSkillStatDic[skilltableid].DeleteMP;
        int SkillDeleteCoolTime = PlayerSkillStatDic[skilltableid].DeleteCoolTime;
        if (SkillCurLevel <= SkillMaxLevel)
        {
            PlayerSkillStatDic[skilltableid].Level += 1;
            PlayerSkillStatDic[skilltableid].SkillAttack += AddSkillAtk;
            PlayerSkillStatDic[skilltableid].SkillAttackPercent += AddSkillAtkper;
            PlayerSkillStatDic[skilltableid].SkillCritical += AddSkillCri;
            PlayerSkillStatDic[skilltableid].NeedMP -= SkillDeleteMp;
            PlayerSkillStatDic[skilltableid].CoolTime -= SkillDeleteCoolTime;
        }
    }

    public static void SkillLevelDown(int tableId)
    {
        SkillType type = PlayerSkillStatDic[tableId].Type;
        switch (type)
        {
            case SkillType.Strengthen:
                StrengthenLevelDown(tableId);
                break;
            case SkillType.Active:
                ActiveLevelDown(tableId);
                break;
        }
    }
    public static void StrengthenLevelDown(int Skilltableid)
    {
        int SkillCurLevel = PlayerSkillStatDic[Skilltableid].Level;
        if (SkillCurLevel <= 1)
            return;
        int SkillAddStack = (int)(PlayerSkillStatDic[Skilltableid].AddTime);
        float SKillAddAttack = PlayerSkillStatDic[Skilltableid].AddAtkPer;
        float SkillAddDefence = PlayerSkillStatDic[Skilltableid].AddDefPer;
        float SkillAddSpeed = PlayerSkillStatDic[Skilltableid].AddSpePer;
        float SkillDeleteMp = PlayerSkillStatDic[Skilltableid].DeleteMP;
        int SkillDeleteCoolTime = PlayerSkillStatDic[Skilltableid].DeleteCoolTime;

        PlayerSkillStatDic[Skilltableid].Level -= 1;
        PlayerSkillStatDic[Skilltableid].SkillTime -= SkillAddStack;
        PlayerSkillStatDic[Skilltableid].AttackPercent -= SKillAddAttack;
        PlayerSkillStatDic[Skilltableid].DefencePercent -= SkillAddDefence;
        PlayerSkillStatDic[Skilltableid].SpeedPercent -= SkillAddSpeed;
        PlayerSkillStatDic[Skilltableid].NeedMP += SkillDeleteMp;
        PlayerSkillStatDic[Skilltableid].CoolTime += SkillDeleteCoolTime;
    }
    public static void ActiveLevelDown(int skilltableid)
    {
        int SkillCurLevel = PlayerSkillStatDic[skilltableid].Level;
        if (SkillCurLevel <= 1)
            return;
        int AddSkillAtk = PlayerSkillStatDic[skilltableid].AddSkillAttack;
        int AddSkillAtkper = PlayerSkillStatDic[skilltableid].AddSkillAttackPercent;
        int AddSkillCri = PlayerSkillStatDic[skilltableid].AddSkillCritical;
        float SkillDeleteMp = PlayerSkillStatDic[skilltableid].DeleteMP;
        int SkillDeleteCoolTime = PlayerSkillStatDic[skilltableid].DeleteCoolTime;

        PlayerSkillStatDic[skilltableid].Level -= 1;
        PlayerSkillStatDic[skilltableid].SkillAttack -= AddSkillAtk;
        PlayerSkillStatDic[skilltableid].SkillAttackPercent -= AddSkillAtkper;
        PlayerSkillStatDic[skilltableid].SkillCritical -= AddSkillCri;
        PlayerSkillStatDic[skilltableid].NeedMP += SkillDeleteMp;
        PlayerSkillStatDic[skilltableid].CoolTime += SkillDeleteCoolTime;
    }

    public static string Text()
    {
        string text = string.Empty;
        text += "ID,SkillName,SkillAttack,SkillCritical,SkillAttackPercent,AddSkillAttack,AddSkillCritical,AddSkillAttackPercent,Type,SkillTime," +
            "AttackPercent,DefencePercent,SpeedPercent,AddTime,AddAtkPer,AddDefPer,AddSpePer,NeedMP,DeleteMP,Level,MaxLevel,NeedPlayerLevel,CoolTime,DeleteCoolTime\n";
        for(int i = 0; i <= PlayerSkillStatDic.Count; i++)
        {
            if (PlayerSkillStatDic.ContainsKey(i))
                text += string.Format(PlayerSkillStatDic[i].ToString());
        }
        return text;
    }

    public static void SaveSkillStat(string path)
    {
        FileStream fs = new FileStream(path,FileMode.Create);
        byte[] buffers = System.Text.Encoding.UTF8.GetBytes(Text());
        fs.Write(buffers, 0, buffers.Length);
        fs.Close();
    }

}
