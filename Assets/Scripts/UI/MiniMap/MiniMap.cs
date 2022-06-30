using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MiniMap :InGameUI
{
    public List<Transform> EnemyPosList = new List<Transform>();
    public List<EnemyIcon> EnemyIconList = new List<EnemyIcon>();
    Dictionary<string, Background> MapBackGroundDic = new Dictionary<string, Background>(); 
    public static float worldWidth;
    public static float worldDepth;
    public static float uiMapWidth;
    public static float uiMapHeight;
    private Background _rpgBackGround;
    private MapIcon mapIcon;
    private EnemyIcon enemyIcon;
    private IconGroup iconGroup;

    GameObject minobject;
    GameObject maxobject;
    GameController game;

    public void Setting()
    {
        if(MapBackGroundDic.Count == 0)
            LoadMapBackGround();
        mapIcon = GetComponentInChildren<MapIcon>();
        if(mapIcon != null)
        {
            mapIcon.Setting(_player.transform);
        }
        //_rpgBackGround = GetComponentInChildren<Background>();
        if (_rpgBackGround != null)
        {
            _rpgBackGround.Init();
            Vector2 sizeDelta = _rpgBackGround.SizeDelta;
            uiMapWidth = sizeDelta.x;
            uiMapHeight = sizeDelta.y;
        }

        iconGroup = GetComponentInChildren<IconGroup>();
        if(iconGroup != null)
        {

        }

        minobject = GameObject.Find("Min");
        maxobject = GameObject.Find("Max");
        if (minobject != null && maxobject != null)
        {
            worldWidth = Mathf.Abs(minobject.transform.position.x - maxobject.transform.position.x);
            worldDepth = Mathf.Abs(minobject.transform.position.z - maxobject.transform.position.z);
        }
        LoadEnemyIcon();
    }

    public void LoadMapBackGround()
    {
        Background[] rgbbackgrounds = GetComponentsInChildren<Background>();
        string stagename = $"{BaseScene.CurrentStage}BackGround";
        for (int i = 0; i < rgbbackgrounds.Length; i++)
        {
            string name = rgbbackgrounds[i].transName;
            if (!MapBackGroundDic.ContainsKey(name))
            {
                MapBackGroundDic.Add(name, rgbbackgrounds[i]);
                if(stagename == name)
                {
                    _rpgBackGround = MapBackGroundDic[name];
                    _rpgBackGround.gameObject.SetActive(true);
                    continue;
                }
                else
                {
                    MapBackGroundDic[name].gameObject.SetActive(false);
                }
            }
        }
    }

    public void MovetoNextMap(string Stages)
    {
        string stagename = $"{Stages}BackGround";

        foreach(string name in MapBackGroundDic.Keys)
        {
            if(stagename == name)
            {
                _rpgBackGround = MapBackGroundDic[stagename];
                _rpgBackGround.gameObject.SetActive(true);
            }
            else
            {
                MapBackGroundDic[name].gameObject.SetActive(false);
            }
        }
        ClearWorld();
    }
    
    public void ClearWorld()
    {
        minobject = null;
        maxobject = null;
    }

    public void LoadEnemyIcon()
    {
        enemyIcon = Resources.Load<EnemyIcon>("Prefabs/UI/EnemyIcon");
    }

    public EnemyIcon IconPooling(GameController gameController,Transform Pos)
    {
        EnemyIcon icon = null;
        game = gameController;
        for (int i = 0; i < EnemyIconList.Count; i++)
        {
            if (EnemyIconList[i].ActiveSelf == false)
            {
                if(gameController.enemies[i].icon != null)
                {
                    icon = EnemyIconList[i];
                    EnemyIconList[i].Active(true);
                    EnemyIconList[i].Setting(Pos);
                }
            }
        }
        if(icon == null)
        {
            EnemyIcon newicon = Instantiate<EnemyIcon>(enemyIcon, iconGroup.transform);
            icon = newicon;
            EnemyIconList.Add(newicon);
            newicon.Active(true);
            newicon.Setting(Pos);
        }
       
        return icon;
    }

    public void Update()
    {
        if(_rpgBackGround != null)
        {
            UIHelper.MarkOnTheRPGGame(_player.transform.position, _rpgBackGround.transform, worldWidth, worldDepth, uiMapWidth, uiMapHeight);
            
        }

        if(game != null)
        {
            foreach (BaseEnemy enemy in game.enemies)
            {
                if (enemy != null)
                {
                    UIHelper.MarkOnAMap(enemy.transform,enemy.icon.transform, worldWidth, worldDepth, uiMapWidth, uiMapHeight);
                }
            }
        }
        
    }
}
