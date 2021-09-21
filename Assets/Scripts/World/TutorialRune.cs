using System;
using UnityEngine;

public class TutorialRune : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
            tutorial.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
            tutorial.SetActive(false);
    }
}