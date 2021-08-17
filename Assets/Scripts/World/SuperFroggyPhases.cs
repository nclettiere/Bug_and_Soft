using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Froggy;
using UnityEngine;

public class SuperFroggyPhases : MonoBehaviour
{
    [SerializeField] private FroggyController superFroggy;
    [SerializeField] private bool isRightSide;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            superFroggy.hallPosition = isRightSide ? 1 : 0;
        }
    }
}
