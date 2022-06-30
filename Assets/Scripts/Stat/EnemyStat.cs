using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStat : Stat
{
    public override void SetStat(int tableindex)
    {
        DefautStat(TableType.CharacterInformation, tableindex);
    }

    public void EnemyStatLevelUp(int tableid)
    {
        switch (tableid)
        {
            case 2:
                MechanicalGolemLevelUp(tableid);
                break;
            case 3:
                KnightLevelUP(tableid);
                break;
            case 4:
                SlayerLevelUP(tableid);
                break;
            case 5:
                DarkElfLevelUP(tableid);
                break;
            case 6:
                WizardLevelUP(tableid);
                break;
        }
    }


    public void KnightLevelUP(int id)
    {
        Level += 1;
        HP += 50;
        MaxHP += 50;
        Attack += 5;
        Defence += 5;
        if (Level % 3 == 0)
            Speed += 1;
        EnemyExp += 25;
        ReSetStat(id);
    }

    public void MechanicalGolemLevelUp(int id)
    {
        Level += 1;
        HP += 100;
        MaxHP += 100;
        Attack += 5;
        Defence += 10;
        if (Level % 5 == 0)
            Speed += 1;
        EnemyExp += 50;
        ReSetStat(id);
    }

    public void DarkElfLevelUP(int id)
    {
        Level += 1;
        HP += 50;
        MaxHP += 50;
        Attack += 5;
        Defence += 5;
        if (Level % 3 == 0)
            Speed += 1;
        EnemyExp += 25;
        ReSetStat(id);
    }

    public void SlayerLevelUP(int id)
    {
        Level += 1;
        HP += 50;
        MaxHP += 50;
        Attack += 5;
        Defence += 5;
        if (Level % 3 == 0)
            Speed += 1;
        EnemyExp += 25;
        ReSetStat(id);
    }
    public void WizardLevelUP(int id)
    {
        Level += 1;
        HP += 50;
        MaxHP += 50;
        Attack += 5;
        Defence += 5;
        if (Level % 3 == 0)
            Speed += 1;
        EnemyExp += 25;
        ReSetStat(id);
    }
    public void BossPase2Stat()
    {
        Attack *= 2;
        Defence *= 2;
        Ciritical += 5.0f;
    }

}
