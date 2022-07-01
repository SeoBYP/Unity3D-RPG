using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapIcon : MiniMap
{
    public Transform target;
    public Image icon;

    //public float worldWidth = 109f;
    //public float worldDepth = 109f;

    public void Setting(Transform player)
    {
        target = player;
        icon = GetComponent<Image>();
    }
    
    private void LateUpdate()
    {
        if(icon != null && target != null)
        {
            UIHelper.MarkOnAMap(target, icon.transform);
        }
    }
}
