using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Froggy;
using UnityEngine;

public class OLC_HallTrigger : MonoBehaviour
{
    private bool hasEnterHall;
    
    [SerializeField] private Collider2D col1;
    [SerializeField] private Collider2D col2;
    [SerializeField] private Animator bossCell;
    [SerializeField] private OLC_Controller olc;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.Instance.ShowBossHealth("Odinn's Lost Crow - Phase 1", olc);
            olc.enabled = true;
            hasEnterHall = true;
            col1.enabled = true;
            bossCell.SetBool("CloseCell", true);
        }
    }
}
