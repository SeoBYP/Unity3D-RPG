using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBossDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        Animation openDoorAni;
        openDoorAni = GetComponent<Animation>();
        GameAudioManager.Instance.Play2DSound("OpenDoor");
        openDoorAni.Play();

    }
}
