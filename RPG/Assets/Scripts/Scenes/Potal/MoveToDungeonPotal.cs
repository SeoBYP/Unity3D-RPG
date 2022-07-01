using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDungeonPotal : BaseScene
{
    ParticleSystem potalEffect;


    public override void Init()
    {
        potalEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //UIManager.Instance.FadeOut();
            MoveToNextScene(Stage.DungeonScene);
        }
    }
}
