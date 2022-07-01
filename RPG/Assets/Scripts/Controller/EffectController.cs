using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public void LoadEffect()
    {
        ParticleSystem particle = Resources.Load<ParticleSystem>("Effects/LevelUp");
        if(particle != null)
        {
            particle = Instantiate(particle, transform.position, transform.rotation);
            particle.transform.position = transform.position;
            particle.Play();
        }
        Destroy(particle.gameObject, 2.5f);
    }
}
