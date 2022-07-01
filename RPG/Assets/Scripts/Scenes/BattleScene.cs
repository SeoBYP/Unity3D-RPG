using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    public GameController gameController;
    public CheckLayer check;
    public bool currStageClear = false;

    WorldUI worldUI;
    private float PrevStageTime = 0;
    private float NextStageTime = 3;

    public override void Init()
    {
        base.Init();
        _player.transform.position = transform.position;
        currStageClear = false;
        gameController.Init();
        check.Init();
        UIManager.Instance.Add<WorldUI>(UIList.WorldUI);
        worldUI = UIManager.Instance.Get<WorldUI>(UIList.WorldUI);
        if (gameController != null)
        {
            worldUI.sliderPooling(gameController);
            worldUI.SetBossHpbar(gameController);
        }
           
    }

    private void Update()
    {
        base.Run();
        worldUI.SetHPEnemy(gameController);
        worldUI.SetHPCamera();
        //worldUI.SetBossInfo(gameController);
        //if (GameController.currEnemycount == 0)
        //{
        //    currStageClear = true;
        //    float elapsedTime = Time.time - PrevStageTime;
        //    if (elapsedTime >= NextStageTime)
        //    {
        //        GameData.Stage++;
        //        GameData.Enemycount++;
        //        PrevStageTime = Time.time;
        //        MoveToNextScene(Stage.TownScene);

        //    }
        //}
    }
}
