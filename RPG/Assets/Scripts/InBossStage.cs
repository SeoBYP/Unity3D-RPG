using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBossStage : MonoBehaviour
{
    Collider _collider;
    private bool IsInBoss = false;
    public void Init()
    {
        _collider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(IsInBoss == false)
            {
                WorldUI world = UIManager.Instance.Get<WorldUI>(UIList.WorldUI);
                world.SetBossInfo();
                IsInBoss = true;
                GameAudioManager.Instance.PlayBacground("BossBattle");
                GameAudioManager.Instance.Play2DSound("BossShouting");
            }
        }
    }
}
