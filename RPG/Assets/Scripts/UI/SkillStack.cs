using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillStack : MonoBehaviour
{
    public Image addstack;
    public Image nonstack;
    public bool IsAddStack;

    public void DefaultSetting(bool State)
    {
        if (State)
        {
            AddStack();
        }
        else
        {
            DeleteStack();
        }
    }

    public void AddStack()
    {
        addstack.gameObject.SetActive(true);
        nonstack.gameObject.SetActive(false);
        IsAddStack = true;
    }

    public void DeleteStack()
    {
        addstack.gameObject.SetActive(false);
        nonstack.gameObject.SetActive(true);
        IsAddStack = false;
    }
}
