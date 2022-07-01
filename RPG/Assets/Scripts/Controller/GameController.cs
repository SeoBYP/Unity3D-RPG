using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameData
{
    private int m_stage = 1;
    private int m_enemyCount = 1;
    private int m_bossCount = 0;
    private int m_curEnemyCount = 0;
    private int m_curBossCount = 0;
    private int m_totalGold = 0;
    private int m_enemyGold = 100;
    private int m_bossGold = 0;
    private int m_playerQuestId = 0;
    private static GameData instance;

    public static GameData Instance
    {
        get
        {
            return instance;
        }
    }
    public int Stage { get { return m_stage; } }
    public int EnemyCount { get { return m_enemyCount; } }
    public int BossCount { get { return m_bossCount; } }
    public int CurEnemyCount { get { return m_curEnemyCount; } }
    public int CurBossCount { get { return m_curBossCount; } }
    public int TotalGold { get { return m_totalGold; } }
    public int PlayerQuestId { get { return m_playerQuestId; } set { m_playerQuestId = value; } }

    public static bool IsClear = false;
    public static bool IsBoss = false;

    public GameData()
    {
        instance = this;
    }

    public void SetStage()
    {
        m_curEnemyCount = m_enemyCount;
        m_bossCount = 0;
        if(m_stage % 5 == 0)
        {
            m_bossCount = 1;
            m_curBossCount = m_bossCount;
        }
        IsClear = false;
    }

    public void StageLevelUp()
    {
        m_stage++;
        m_enemyCount++;
        m_enemyGold += 100;
        if (m_stage % 5 == 0)
        {
            m_bossGold += 500;
        }
    }

    public void DeleteEnemyCount()
    {
        if(m_curEnemyCount >= 1)
        {
            m_curEnemyCount--;
            
        }
        if (IsBoss)
        {
            if (m_curBossCount >= 1)
            {
                m_curBossCount--;
            }
        }
        IsClear = CheckStageClear();
    }

    public bool CheckStageClear()
    {
        if (m_curEnemyCount == 0)
        {
            if(m_curBossCount == 0)
            {
                IsBoss = false;
                Debug.Log(IsBoss);
                return true;
            }
            else
            {
                IsBoss = true;
                Debug.Log(IsBoss);
                return false;
            }
        }
        return false;
    }

    public void SetStageReword()
    {
        int enemygold = m_enemyCount * m_enemyGold;
        int bossgold = m_bossCount * m_bossGold;
        m_totalGold = enemygold + bossgold;
    }
}

public class GameController : MonoBehaviour
{
    public Dictionary<int, BaseEnemy> EnemyDic = new Dictionary<int, BaseEnemy>();
    public List<BaseEnemy> enemies = new List<BaseEnemy>();
    public List<BaseEnemy> BossEnemies = new List<BaseEnemy>();

    public List<Transform> EnemiesPosList = new List<Transform>();
    public Transform EnemyPosGroup;
    public Transform BossEnemyPos;
    public DungeonScene scene;
    public InGameUI inGame;
    public WorldUI worldUI;

    public OpenBossDoor rightDoor;
    public OpenBossDoor leftDoor;

    public static bool currStageClear = false;

    public void Init()
    {
        
        GameData.Instance.SetStage();
        GetEnemyPosList();
        LoadEnemy();
        Pooling();
        LoadBossEnemy();
        IgnoreCollider();
        inGame = UIManager.Instance.Get<InGameUI>(UIList.InGameUI);
        inGame.SetGameStageInfo(BaseScene.CurrentStage);
    }
    
    public void IgnoreCollider()
    {
        //Physics.IgnoreLayerCollision(int layerIndex , int layerIndex) : ???????? ?? ???? ???????? ???? ?????? ????????. 
        Physics.IgnoreLayerCollision(8, 7);
        //Physics.IgnoreCollision(collider, collider) : ???????? ?? ???? ?????????? ???? ?????? ????????. 
    }

    public void GetEnemyPosList()
    {
        if(EnemyPosGroup != null)
        {
            for(int i = 0; i < EnemyPosGroup.childCount; i++)
            {
                EnemiesPosList.Add(EnemyPosGroup.GetChild(i));
            }
        }
    }
    
    private BaseEnemy[] Pooling()
    {
        BaseEnemy[] enemy = null;
        int enemyposmaxcount = EnemiesPosList.Count - 1;
        int RandomEnemyIndex = EnemyDic.Count - 1;
        int enemycurcount = GameData.Instance.CurEnemyCount;

        for(int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].ActiveSelf)
            {
                enemies[i].gameObject.SetActive(true);
                enemies[i].Init();
            }
        }

        for (int i = 0; i < enemycurcount; i++)
        {
            int randomPos = Random.Range(0, enemyposmaxcount);
            int RandomEnemy = Random.Range(0, RandomEnemyIndex);
            if (enemy == null)
            {
                enemies.Add(Instantiate<BaseEnemy>(EnemyDic[RandomEnemy], EnemiesPosList[randomPos]));
                if(EnemiesPosList[randomPos] != null)
                {
                    enemies[i].StartPos = EnemiesPosList[randomPos].position;
                    enemies[i].transform.position = EnemiesPosList[randomPos].position;
                    enemies[i].Init();
                }
                
                else
                {
                    enemies[i].transform.position = transform.position;
                    print("Don't Get EnemyPosList");
                }
            }
            
        }
        return enemy;
    }

    public void LoadEnemy()
    {
        BaseEnemy[] baseEnemies = Resources.LoadAll<BaseEnemy>("Prefabs/Enemy");
        for(int i = 0; i < baseEnemies.Length; ++i)
        {
            if(EnemyDic.ContainsKey(i) == false)
            {
                EnemyDic.Add(i, baseEnemies[i]);
            }
        }
    }

    public void LoadBossEnemy()
    {
        int bosscurcount = GameData.Instance.CurBossCount;
        if (bosscurcount < 1)
            return;
        BaseEnemy enemy = Resources.Load<BaseEnemy>("Prefabs/BossEnemy/MechanicalGolem");
        if(enemy != null)
        {
            BossEnemies.Add(Instantiate<BaseEnemy>(enemy, BossEnemyPos));
            for(int i = 0; i < bosscurcount; i++)
            {
                if(BossEnemies[i].gameObject != null)
                {
                    if(BossEnemyPos != null)
                    {
                        BossEnemies[i].StartPos = BossEnemyPos.position;
                        BossEnemies[i].Init();
                    }
                    
                }
            }
        }
    }

    public void CheckQuestCondition(string condition)
    {
        int currentplayerquest = 0;
        foreach(int index in Quest.QuestInfoDic.Keys)
        {
            if (Quest.QuestInfoDic[index].Condition == condition)
            {
                currentplayerquest = Quest.QuestInfoDic[index].ID;
                Debug.Log(currentplayerquest);
            }
        }

        if (currentplayerquest == GameData.Instance.PlayerQuestId)
        {
            Quest.CheckCondition(condition, currentplayerquest);
            inGame.ReSetIngameQuestList(currentplayerquest);
            PlayerQuestPopupUI playerQuest = UIManager.Instance.Get<PlayerQuestPopupUI>(UIList.PlayerQuestPopupUI);
            if (playerQuest != null)
                playerQuest.ReSetQuestSlotList(currentplayerquest);
        }
    }

    public void EnemyLevelUp()
    {
        EnemyStat enemyStat = GetComponent<EnemyStat>();
        int count = DataManager.TableDic[TableType.CharacterInformation].InfoDic.Count;
        for (int i = 0; i < count; i++)
        {
            enemyStat.EnemyStatLevelUp(i);
        }
    }

    public void CheckClearStage()
    {
        GameData.Instance.DeleteEnemyCount();
        inGame.SetGameStageInfo(BaseScene.CurrentStage);

        if (GameData.IsBoss)
        {
            rightDoor.OpenDoor();
            leftDoor.OpenDoor();
        }

        if (GameData.IsClear)
        {
            //worldUI = UIManager.Instance.Get<WorldUI>(UIList.WorldUI);
            inGame.IsSetGameStage = false;
            inGame.SetStageClear();
            GameData.Instance.StageLevelUp();
            if(GameData.Instance.Stage % 2 == 0)
            {
                EnemyLevelUp();
            }
            GameData.IsClear = false;
            CheckQuestCondition("Stage");
        }
    }
}
