using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MonoBehaviour
{
    public Transform target;
    public Image icon;

    public bool ActiveSelf { get { return gameObject.activeSelf; } }

    public void Setting(Transform Enemy)
    {
        target = Enemy;
        icon = GetComponent<Image>();
    }

    public void Active(bool state)
    {
        this.gameObject.SetActive(state);
    }
}
