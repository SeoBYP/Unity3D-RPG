using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStageInfo : MonoBehaviour
{
    public TMP_Text gamestagecount;
    public TMP_Text gameenemycount;
    public TMP_Text gamebosscount;

    public void DeActive()
    {
        transform.gameObject.SetActive(false);
    }

    public void Active()
    {
        transform.gameObject.SetActive(true);
    }

    public void SetActive()
    {
        if(this.gameObject.activeSelf == false)
        {
            Active();
        }
        SetGameStage();
        SetEnemyCount();
        SetBossCount();
    }

    public void SetGameStage()
    {
        int gamestage = GameData.Instance.Stage;
        gamestagecount.text = gamestage.ToString();
    }

    public void SetEnemyCount()
    {
        int gamecurenemy = GameData.Instance.CurEnemyCount;
        int gameenemy = GameData.Instance.EnemyCount;
        gameenemycount.text = $"{gamecurenemy} / {gameenemy}";
    }

    public void SetBossCount()
    {
        int gamecurboss = GameData.Instance.CurBossCount;
        int gameboss = GameData.Instance.BossCount;
        gamebosscount.text = $"{gamecurboss} / {gameboss}";
    }
}
