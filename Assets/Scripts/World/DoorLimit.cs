using System;
using UnityEngine;

public class DoorLimit : MonoBehaviour
{
    private bool hasEnterHall;
    
    [SerializeField] private Collider2D col1;
    [SerializeField] private Collider2D col2;
    [SerializeField] private Animator bossCell;

    private void Start()
    {
        GameManager.Instance.OnLevelReset.AddListener(DisableLimits);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            hasEnterHall = true;
            col1.enabled = true;
            bossCell.SetBool("CloseCell", true);
        }
    }

    private void DisableLimits()
    {
        
        hasEnterHall = false;
        col1.enabled = false;
        bossCell.SetBool("CloseCell", false);
    }
}