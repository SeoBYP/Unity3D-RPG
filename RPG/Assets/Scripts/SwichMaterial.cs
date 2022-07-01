using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichMaterial : MonoBehaviour
{
    Renderer transRenderer;
    Material transMaterial;

    public Material lagecymaterial;
    private void Start()
    {
        transRenderer = GetComponentInChildren<Renderer>();
        transMaterial = transRenderer.material;
    }

    public void Swich()
    {
        transRenderer.material = lagecymaterial;

    }

    public void SetDefault()
    {
        transRenderer.material = transMaterial;
    }
}
