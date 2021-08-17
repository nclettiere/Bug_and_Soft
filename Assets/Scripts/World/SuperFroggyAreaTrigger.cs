using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using UI;
using UnityEngine;

public class SuperFroggyAreaTrigger : MonoBehaviour
{
    [SerializeField] private FroggyController superFroggy;
    [SerializeField] private UI_HUD hud;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            superFroggy.enabled = true;
            if (superFroggy.FacingDirection == -1)
                superFroggy.Flip();
            GameManager.Instance.ShowBossHealth("SUPER FROGGY", superFroggy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.Instance.HideBossHealth();
        }
    }
}