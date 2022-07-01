using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Util.CreateObject<T>(null, true);
          
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    public virtual void Init()
    {

    }
    public virtual void Release()
    {
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    protected Manager()
    {

    }
}
