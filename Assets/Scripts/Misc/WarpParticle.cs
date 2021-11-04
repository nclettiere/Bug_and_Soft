using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Damage;
using Player;
using UnityEngine;

public class WarpParticle : MonoBehaviour
{
    [SerializeField] private Animator bombAnimator;
    [SerializeField] private int damageAmount = 5;
    [SerializeField] private float damageRadius = 3f;
    [SerializeField] private LayerMask whatIsPlayer;
    public void AnimOnParticleEnd()
    {
        Destroy(gameObject);
    }
    
    public void DoDamage()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, damageRadius, Vector2.zero, 0f, whatIsPlayer);

        if (hit != null)
        {
            if (hit.transform != null)
            {
                PlayerController bctrl = hit.transform.GetComponent<PlayerController>();
                if (bctrl != null && (bctrl is IDamageable))
                {
                    DamageInfo damageInfo = new DamageInfo(damageAmount, transform.position.x, true);
                    bctrl.Damage(damageInfo);
                }
            }
        }
    }

    public void AnimOnBombEnd()
    {
        bombAnimator.SetBool("Explode", true);
    }
}