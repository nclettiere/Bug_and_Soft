using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class VergenHeadAsesina : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool playerHit;
    public float Speed = 900;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            rb.velocity = transform.right * Speed;

            if (playerHit)
            {
                Vector2 moveDirection = rb.velocity;
                if (moveDirection != Vector2.zero)
                {
                    float angle = Mathf.Atan2(10, moveDirection.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var pController = other.transform.GetComponent<PlayerController>();

            if (!pController.rollAnim)
            {
                rb.gravityScale = -3;
                playerHit = true;
                GameManager.Instance.VergenTrapStart();
                Destroy(gameObject, 5f);
            }
        }
    }
}