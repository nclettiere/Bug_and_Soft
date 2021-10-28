using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Damage;
using Player;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    
    [SerializeField] private int damage = 5;
    [SerializeField] private float damageCooldown = 2f;
    private float damageCooldownTime = float.NegativeInfinity;
    private DamageInfo dInfo;
    private bool canInflictDamage;

    private List<Transform> damageList;

    private void Start()
    {
        damageList = new List<Transform>();
        dInfo = new DamageInfo(damage, transform.position.x, true);
        dInfo.MoveOnAttackForce = new Vector2(10, 15);
    }

    private void FixedUpdate()
    {
        if (damageList.Count > 0)
        {
            foreach (var transform in damageList)
            {
                if (Time.time >= damageCooldownTime)
                {
                    if (transform.CompareTag("Player"))
                    {
                        transform.GetComponent<PlayerController>()
                            .Damage(dInfo);
                        damageCooldownTime = Time.time + damageCooldown;
                    }
                    else if (transform.CompareTag("Enemy"))
                    {
                        BaseController bc = transform.GetComponent<BaseController>();
                        if (bc != null)
                        {
                            bc.Damage(dInfo);
                        }

                        damageCooldownTime = Time.time + damageCooldown;
                    }
                }   
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        damageList.Add(other.transform);
        canInflictDamage = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        damageList.Remove(other.transform);
        canInflictDamage = false;
    }
}
