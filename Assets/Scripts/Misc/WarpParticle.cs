using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpParticle : MonoBehaviour
{
    public void AnimOnParticleEnd()
    {
        Destroy(gameObject);
    }
}