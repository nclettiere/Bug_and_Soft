using System;
using UnityEngine;

namespace Items
{
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField] private int lifetime = 20;
        [SerializeField] private bool isPercenatage;
        [SerializeField] private int healingAmount = 50;
        [SerializeField] private float lifetimeWait;

        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private int overlapRadius;
        
        private void Start()
        {
            lifetimeWait = Time.time + lifetime;
        }

        private void Update()
        {
            if (GameManager.Instance.DeltaTime > 0f)
            {
                if (Time.time >= lifetimeWait)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void CheckPlayerNearby()
        {
            var hit = Physics2D.OverlapCircle(transform.position, overlapRadius, whatIsPlayer);

            if (hit != null)
            {
                
            }
        }
    }
}