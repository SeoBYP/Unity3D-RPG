using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//????????
public class WorldUI : BaseUI
{
    public List<HP_Bar> EnemiesHp = new List<HP_Bar>();
   // public List<int> removeList = new List<int>();

    Canvas canvas;
    public Image BossStageBg;
    public Image BossInfo;
    public Transform IngameBossInfo;
    private BossHpUI BossHpui;
    private HP_Bar hp;
    private bool IsBossHpActive = false;
    public override void Init()
    {
        canvas = GetComponent<Canvas>();
        BossHpui = GetComponentInChildren<BossHpUI>();
        BossHpui.SetActive(false);
        BossStageBg.gameObject.SetActive(false);
        BossInfo.gameObject.SetActive(false);
        LoadHp();
        //IngameBossInfo.gameObject.SetActive(false);
    }

    public void LoadHp()
    {
        if (hp == null)
            hp = Resources.Load<HP_Bar>("Prefabs/UI/HP_Bar");
    }

    IEnumerator InBossStage()
    {
        BossStageBg.gameObject.SetActive(true);
        BossInfo.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        BossStageBg.gameObject.SetActive(false);
        BossInfo.gameObject.SetActive(false);
        IngameBossInfo.gameObject.SetActive(true);
        IsBossHpActive = true;
    }

    public void SetBossInfo()
    {
        StartCoroutine(InBossStage());
    }

    public void SetHPCamera()
    {
        for (int i = 0; i < EnemiesHp.Count; i++)
        {
            if (CheckLayer.CheckEnemycast)
            {
                if (EnemiesHp[i].gameObject != null)
                {
                    if (EnemiesHp[i].IsDead == false)
                    {
                        EnemiesHp[i].gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (EnemiesHp[i].gameObject != null)
                {
                    EnemiesHp[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public HP_Bar sliderPooling(GameController gameController)
    {
        HP_Bar newhp = null;
        for (int i = 0; i < EnemiesHp.Count; i++)
        {
            if (EnemiesHp[i].ActiveSelf == false)
            {
                if (gameController.enemies[i].HP != null)
                {
                    newhp = EnemiesHp[i];
                    EnemiesHp[i].Init();
                }
                //if (gameController.enemies[i] != null)
                //     gameController.enemies[i].SetHP(EnemiesHp[i]);
            }
        }
        if (newhp == null)
        {
            HP_Bar _hp = Instantiate<HP_Bar>(hp, gameObject.transform);
            EnemiesHp.Add(_hp); 
            newhp = _hp;
            _hp.Init();
            //gameController.enemies.SetHP(_hp);
        }
        return newhp;
    }

    public void SetBossHpbar(GameController gameController)
    {
        int count = gameController.BossEnemies.Count;
        if (count < 1)
            return;
        for (int i = 0; i < count; i++)
        {
            if (gameController.BossEnemies[i].gameObject != null)
            {
                gameController.BossEnemies[i].SetBossHP(BossHpui);
                BossHpui.Init();
                return;
            }
            else
            {
                gameController.BossEnemies[i].SetBossHP(BossHpui);
                BossHpui.Init();
                return;
            }
        }
        //BossHpui.SetActive(false);
    }


    public void SetHPEnemy(GameController gameController)
    {
        //for (int i = 0; i < EnemiesHp.Count; i++)
        //{
        //    if (EnemiesHp[i] != null)
        //    {
        //        if (gameController.enemies[i] != null)
        //        {
        //            float height = gameController.enemies[i].transform.localScale.y;
        //            Vector3 tranVec = gameController.enemies[i].transform.position + new Vector3(0, height, 0);
        //            EnemiesHp[i].Execute(tranVec);
        //        }
        //    }
        //}

        for(int i = 0; i < gameController.enemies.Count; i++)
        {
            if(gameController.enemies[i].HP != null)
            {
                HP_Bar hp = gameController.enemies[i].HP;
                float height = gameController.enemies[i].transform.localScale.y;
                Vector3 tranVec = gameController.enemies[i].transform.position + new Vector3(0, height, 0);
                hp.Execute(tranVec);
            } 
        }
    }

    public void EnemyHPRemove()
    {
        for (int i = 0; i < EnemiesHp.Count; i++)
        {
            if (EnemiesHp[i] != null)
            {
                EnemiesHp[i].DeAcitve();
            }
        }
    }

    public void BossHPRemove()
    {
        if (BossHpui != null)
        {
            BossHpui.SetActive(false);
        }
    }

}



