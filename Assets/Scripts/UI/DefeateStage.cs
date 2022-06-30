using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeateStage : BaseScene
{
    public Transform stageDefeate;
    public Button BacktoTown;

    private Animation defeateAni;

    public void Inititate()
    {
        transform.localPosition = new Vector3(0, 3000, 0);
        defeateAni = stageDefeate.GetComponent<Animation>();
        BacktoTown.onClick.AddListener(MovetoTown);
    }

    public void Excute()
    {
        GameAudioManager.Instance.Play2DSound("Defeate");
        transform.localPosition = Vector3.zero;
        defeateAni.Play();
    }

    public void Close()
    {
        transform.localPosition = new Vector3(0, 3000, 0);
    }

    public void MovetoTown()
    {
        MoveToNextScene(Stage.TownScene);
        Close();
    }

    
}
