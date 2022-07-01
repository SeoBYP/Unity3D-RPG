using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : BaseScene
{
    public GameController gameController;
    public CheckLayer check;
    public InBossStage inBossStage;

    WorldUI worldUI;

    public override void Init()
    {
        base.Init();
        _player.transform.position = transform.position;
        if (_ingameui != null)
        {
            _ingameui.miniMap.Setting();
        }
        gameController.Init();
        check.Init();
        UIManager.Instance.Add<WorldUI>(UIList.WorldUI);
        worldUI = UIManager.Instance.Get<WorldUI>(UIList.WorldUI);
        if (gameController != null)
        {
            for(int i = 0; i < gameController.enemies.Count; i++)
            {
                if(gameController.enemies[i].HP == null)
                {
                    gameController.enemies[i].SetHP(worldUI.sliderPooling(gameController));
                }
            }

            worldUI.SetBossHpbar(gameController);
        }
        if (inBossStage != null)
            inBossStage.Init();
        GameAudioManager.Instance.PlayBacground("The_Wandering_Hero_Version_01_LOOP");
        SetEnemyIcon();
    }

    public void SetEnemyIcon()
    {
        _ingameui.SetMiniMapIcon(gameController);

    }


    private void Update()
    {
        base.Run();
        worldUI.SetHPEnemy(gameController);
        worldUI.SetHPCamera();
    }
}
