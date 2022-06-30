using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageClear : BaseScene
{
    private Canvas canvas;
    private Animation stageclearani;
    private List<Animation> staranimations = new List<Animation>();
    private List<Animation> stageScoreAnis = new List<Animation>();

    public Transform Stars;
    public Transform stagescore;
    public Transform stagecleartext;
    public Button nextStage;
    public TMP_Text stagecount;
    public TMP_Text EnemyCount;
    public TMP_Text DeleteEnemyCount;
    public TMP_Text Reword;

    public static bool IsStageClear = false;

    public void Inititate()
    {
        stageclearani = stagecleartext.GetComponent<Animation>();
        stagecleartext.localScale = Vector3.zero;
        GetStarsAni();
        GetStageScoreAni();
        nextStage.onClick.AddListener(MovetoTown);
        transform.localPosition = new Vector3(0, 3000, 0);
    }

    void GetStarsAni()
    {
        if(Stars != null)
        {
            for(int i = 0; i < Stars.childCount; i++)
            {
                Transform newstar = Stars.GetChild(i);
                newstar.localScale = Vector3.zero;
                Animation starani = newstar.GetComponent<Animation>();
                if(starani != null)
                {
                    staranimations.Add(starani);
                }
            }
        }
    }

    void GetStageScoreAni()
    {
        if (stagescore != null)
        {
            for (int i = 0; i < stagescore.childCount; i++)
            {
                Transform newstageScore = stagescore.GetChild(i);
                newstageScore.localScale = Vector3.zero;
                Animation stagescoreani = newstageScore.GetComponent<Animation>();
                //newstageScore.localScale = Vector3.zero;
                if (stagescoreani != null)
                {
                    stageScoreAnis.Add(stagescoreani);
                }
            }
        }
    }

    public void Close()
    {
        transform.localPosition = new Vector3(0, 3000, 0);
        IsStageClear = false;
    }

    public void Excute(float speed,bool state)
    {
        transform.localPosition = Vector3.zero;
        IsStageClear = true;
        SetText();
        GameAudioManager.Instance.Play2DSound("Clear");
        StartCoroutine(TextExcute());
    }

    IEnumerator TextExcute()
    {
        yield return new WaitForSeconds(0.5f);
        stageclearani.Play();
        yield return new WaitForSeconds(0.3f);
        for(int i = 0; i < staranimations.Count; i++)
        {
            staranimations[i].Play();
            yield return new WaitForSeconds(0.3f);
        }
        for(int i = 0; i < stageScoreAnis.Count; i++)
        {
            stageScoreAnis[i].Play();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetText()
    {
        int enemycount = GameData.Instance.EnemyCount + GameData.Instance.BossCount;
        int deletecount = GameData.Instance.EnemyCount + GameData.Instance.BossCount; ;
        int reword = GameData.Instance.TotalGold;
        int stage = GameData.Instance.Stage;

        stagecount.text = $"{stage} STAGE";
        EnemyCount.text = $"EnemyCount : {enemycount}";
        DeleteEnemyCount.text = $"DeleteEnemyCount : {deletecount}";
        Reword.text = $"StageReword : {reword}";
    }

    public void MovetoTown()
    {
        MoveToNextScene(Stage.TownScene);
        Close();
    }

}
