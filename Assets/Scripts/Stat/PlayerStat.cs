using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerStat : Stat
{
    private bool IsDefaltSet = false;
    public override void SetStat(int tableindex)
    {
        if (!IsDefaltSet)
        {
            DefautStat(TableType.CharacterInformation, tableindex);
            IsDefaltSet = true;
        }
    }

    public void PlayerLevelUP()
    {
        if (Exp >= MaxExp)
        {
            Level += 1;
            MaxHP += 30;
            MaxMP += 20;
            HP = MaxHP;
            MP = MaxMP;
            Attack += 5;
            Defence += 5;
            if (Level % 5 == 0)
            {
                Speed += 1;
            }
            Exp = Exp - MaxExp;
            MaxExp += 100;
            SkillStack++;
        }
        ReSetStat(ID);
    }

    public void DeletePlayerGold(int coin)
    {
        Gold -= coin;
        ReSetStat(ID);
    }

    public void AddPlayerGold(int coin)
    {
        Gold += coin;
        ReSetStat(ID);
    }

    public void SaveInfo(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ToString());
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();
    }
    //파일 스트림을 새로 생
}
