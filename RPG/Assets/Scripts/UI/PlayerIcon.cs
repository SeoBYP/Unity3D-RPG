using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
