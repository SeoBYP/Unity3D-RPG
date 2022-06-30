using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPooling : MonoBehaviour
{
    private Arrow prefab;
    private List<Arrow> arrowlist = new List<Arrow>();
    private static ArrowPooling instance;

    public static ArrowPooling Instance
    {
        get { return instance; }
    }

    public void Init()
    {
        instance = this;
        prefab = Resources.Load<Arrow>("Prefabs/Weapons/ArrowPrefab");
    }

    public Arrow Pooling()
    {
        Arrow arrow = null;
        for(int i = 0; i < arrowlist.Count; i++)
        {
            if(arrowlist[i].ActiveSelf == false)
            {
                arrow = arrowlist[i];
                arrow.SetActive(true);
            }
        }
        if(arrow == null)
        {
            Arrow arrowprefab = Instantiate(prefab);
            if(arrowprefab != null)
            {
                arrowprefab.Init();
                arrowlist.Add(arrowprefab);
                arrow = arrowprefab;
            }
        }
        return arrow;
    }
}
