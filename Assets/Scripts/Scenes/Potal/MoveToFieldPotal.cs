using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFieldPotal : BaseScene
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
            //UIManager.Instance.FadeIn();
            MoveToNextScene(Stage.FieldScene);
        }
    }
}
