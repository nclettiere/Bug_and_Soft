using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Froggy;
using UI;
using UnityEngine;

public class SuperFroggyAreaTrigger : MonoBehaviour
{
    [SerializeField] private GameObject superFroggyContainer;
    [SerializeField] private UI_HUD hud;
    
    
    private BaseController superFroggy;

    private void Start()
    {
        if (!superFroggyContainer.TryGetComponent(out superFroggy))
        {
            EnemySpawner spawner;
            // Asumimos que se trata de un spawner
            if (superFroggyContainer.TryGetComponent(out spawner))
            {
                superFroggy = spawner.GetController();
            }
            else
            {
                Debug.LogError("No se pudo encontrar el FroggyController dentro del container...");
            }
        }

        Debug.Assert(superFroggy != null);
    }

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