using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DelayTime : MonoBehaviour
{
    
}

public class Util
{
    public static T GetOrAddComponent<T>(GameObject origin) where T : Component
    {
        T Component = origin.GetComponent<T>();
        if(Component == null)
        {
            Component = origin.AddComponent<T>();
        }
        return Component;
    }

    public static T FindObject<T>(Transform transform, string path, bool init = false) where T : Component
    {
        Transform trans = transform.Find(path);
        T findobject = null;
        if(trans != null)
        {
            findobject = trans.GetComponent<T>();
            if (init)
                trans.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
        }
        return findobject;
    }
    public static T CreateObject<T>(Transform parent,bool init = false) where T : Component
    {
        GameObject obj = new GameObject(typeof(T).Name, typeof(T));
        obj.transform.SetParent(parent);
        T t = obj.GetComponent<T>();
        if(init)
            t.SendMessage("Init", SendMessageOptions.DontRequireReceiver);

        return t;
    }

    public static Button BindingFunc(Transform transform,UnityAction action)
    {
        Button button = transform.GetComponentInChildren<Button>();
        if(button != null)
        {
            button.onClick.AddListener(action);
        }
        return button;
    }

    public static Button BindingFunc(Transform parent, string path, UnityAction action)
    {
        Button button = FindObject<Button>(parent, path);
        if(button != null)
        {
            button.onClick.AddListener(action);
        }
        return button;
    }

    public static T Instantiate<T>(string path,Transform parent,bool init = false, bool active = true) where T : UnityEngine.Component
    {
        T objectType = Resources.Load<T>(path);
        if(objectType != null)
        {
            objectType = Object.Instantiate(objectType);
            if(objectType != null)
            {
                if (init)
                    objectType.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
                objectType.gameObject.SetActive(active);
            }
        }
        else if(objectType == null)
        {
            Debug.Log("Don't Instantiate Gameobject");
        }
        return objectType;
    }

    
}
