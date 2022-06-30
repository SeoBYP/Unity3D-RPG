using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMashRender : MonoBehaviour
{
    Renderer[] renderers;

    public void GetRenderer()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void OnHited()
    {
        StartCoroutine(SetColor());
    }

    IEnumerator SetColor()
    {
        foreach(Renderer mesh in renderers)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);

        foreach(Renderer mesh in renderers)
        {
            mesh.material.color = Color.white;
        }
    }
}
