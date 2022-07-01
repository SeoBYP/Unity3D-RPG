using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Stat : MonoBehaviour
{
    public static Dictionary<int, Stat> StatDic = new Dictionary<int, Stat>();

    [SerializeField]
    private int _ID;
    [SerializeField]
    private string _Name;
    [SerializeField]
    private float _HP;
    [SerializeField]
    private float _MaxHP;
    [SerializeField]
    private float _Attack;
    [SerializeField]
    private float _Critical;
    [SerializeField]
    private float _Defence;
    [SerializeField]
    private float _Speed;
    [SerializeField]
    private int _Level;
    [SerializeField]
    private float _Exp;
    [SerializeField]
    private float _MaxExp;
    [SerializeField]
    private int _Gold = 0;
    [SerializeField]
    private float _JumpPower;
    [SerializeField]
    private int _EnemyEXP;
    [SerializeField]
    private float _Mp;
    [SerializeField]
    private float _MaxMp;
    [SerializeField]
    private int _skillStack;

    public int ID { get { return _ID; } set { _ID = value; } }
    public string Name { get { return _Name; } set { _Name = value; } }
    public int Level { get { return _Level; } set { _Level = value; } }
    public float HP { get { return _HP; } set { _HP = value; } }
    public float MaxHP { get { return _MaxHP; } set { _MaxHP = value; } }
    public float Attack { get { return _Attack; } set { _Attack = value; } }
    public float Ciritical { get { return _Critical; } set { _Critical = value; } }
    public float Defence { get { return _Defence; } set { _Defence = value; } }
    public float Speed { get { return _Speed; } set { _Speed = value; } }
    public float JumpPower { get { return _JumpPower; } set { _JumpPower = value; } }
    public float Exp { get { return _Exp; } set { _Exp = value; } }
    public float MaxExp { get { return _MaxExp; } set { _MaxExp = value; } }
    public int Gold { get { return _Gold; } set { _Gold = value; } }
    public int EnemyExp { get { return _EnemyEXP; } set { _EnemyEXP = value; } }
    public float MP { get { return _Mp; } set { _Mp = value; } }
    public float MaxMP { get { return _MaxMp; } set { _MaxMp = value; } }
    public int SkillStack { get { return _skillStack; } set { _skillStack = value; } }

    public void DefautStat(TableType Type, int tableindex)
    {
        _ID = tableindex;//DataManager.ToInter(Type, tableindex, "_ID");
        _Name = DataManager.ToString(Type, tableindex, "_Name");
        _Level = DataManager.ToInter(Type, tableindex, "_Level");
        _HP = DataManager.ToFloat(Type, tableindex, "_HP");
        _MaxHP = DataManager.ToFloat(Type, tableindex, "_MaxHP");
        _Attack = DataManager.ToFloat(Type, tableindex, "_Attack");
        _Critical = DataManager.ToFloat(Type, tableindex, "_Critical");
        _Defence = DataManager.ToFloat(Type, tableindex, "_Defence");
        _Speed = DataManager.ToFloat(Type, tableindex, "_Speed");
        _JumpPower = DataManager.ToFloat(Type, tableindex, "_JumpPower");
        _Exp = DataManager.ToFloat(Type, tableindex, "_Exp");
        _MaxExp = DataManager.ToFloat(Type, tableindex, "_MaxExp");
        _Gold = DataManager.ToInter(Type, tableindex, "_Gold");
        _EnemyEXP = DataManager.ToInter(Type, tableindex, "_EnemyEXP");
        _Mp = DataManager.ToFloat(Type, tableindex, "_Mp");
        _MaxMp = DataManager.ToFloat(Type, tableindex, "_MaxMp");
        _skillStack = 1;
    }

    public void ReSetStat(int tableindex)
    {
        if (StatDic.ContainsKey(tableindex))
        {
            StatDic[tableindex].ID = _ID;
            StatDic[tableindex].Name = _Name;
            StatDic[tableindex].Level = _Level;
            StatDic[tableindex].HP = _HP;
            StatDic[tableindex].MaxHP = _MaxHP;
            StatDic[tableindex].Attack = _Attack;
            StatDic[tableindex].Ciritical = _Critical;
            StatDic[tableindex].Defence = _Defence;
            StatDic[tableindex].Speed = _Speed;
            StatDic[tableindex].JumpPower = _JumpPower;
            StatDic[tableindex].Exp = _Exp;
            StatDic[tableindex].MaxExp = _MaxExp;
            StatDic[tableindex].Gold = _Gold;
            StatDic[tableindex].EnemyExp = _EnemyEXP;
            StatDic[tableindex].MP = _Mp;
            StatDic[tableindex].MaxMP = _MaxMp;
        }
    }

    public override string ToString()
    {
        string text = string.Empty;
        text += string.Format("_ID,_Name,_Level,_HP,_MaxHP,_Attack,_Critical,_Defence,_Speed,_JumpPower,_Exp,_MaxExp,_Gold,_EnemyEXP,_Mp,_MaxMp\n");
        for (int i = 0; i <= StatDic.Count; i++)
        {
            //ReSetStat(i);
            if (StatDic.ContainsKey(i))
            {
                string newid = StatDic[i].ID.ToString();
                string newname = StatDic[i].Name;
                string newlevel = StatDic[i].Level.ToString();
                string newhp = StatDic[i].HP.ToString();
                string newmaxhp = StatDic[i].MaxHP.ToString();
                string newattack = StatDic[i].Attack.ToString();
                string newcritical = StatDic[i].Ciritical.ToString();
                string newdefence = StatDic[i].Defence.ToString();
                string newspeed = StatDic[i].Speed.ToString();
                string newjump = StatDic[i].JumpPower.ToString();
                string newexp = StatDic[i].Exp.ToString();
                string newmaxexp = StatDic[i].MaxExp.ToString();
                string newgold = StatDic[i].Gold.ToString();
                string newenemyexp = StatDic[i].EnemyExp.ToString();
                string newmp = StatDic[i].MP.ToString();
                string newmaxmp = StatDic[i].MaxMP.ToString();
                text += string.Format($"{newid},{newname},{newlevel},{newhp},{newmaxhp},{newattack},{newcritical},{newdefence},{newspeed},{newjump},{newexp},{newmaxexp},{newgold},{newenemyexp},{newmp},{newmaxmp}\n");
            }
        }
        return text;
    }

    public virtual void SetStat(int tableindex)
    {
        
    }

    public static void StatData()
    {
        if (DataManager.TableDic.ContainsKey(TableType.CharacterInformation))
        {
            int index = DataManager.TableDic[TableType.CharacterInformation].InfoDic.Count;
            for(int i = 1; i <= index; i++)
            {
                if (!StatDic.ContainsKey(i))
                {
                    StatDic.Add(i, new Stat());
                    StatDic[i].DefautStat(TableType.CharacterInformation, i);
                }
            }
        }
    }

    public byte[] GetByteArr(FileStream fp)
    {
        byte[] bytes = null;
        try
        {
            bytes = new byte[fp.Length];
            fp.Read(bytes, 0, (int)fp.Length);
        }
        catch (IOException exception)
        {
            print(exception);
        }
        return bytes;
    }

    public void SaveStatData(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ToString());
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();
    }
}
