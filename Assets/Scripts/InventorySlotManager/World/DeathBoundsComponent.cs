using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBoundsComponent : MonoBehaviour
{
    [SerializeField] private AudioSource deathSFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.KillPlayer(other.transform);
            deathSFX.Play();
        }else if (other.CompareTag("Enemy"))
        {
            Debug.Log("GOT AN ENEMY");
            // Kill ... TODO
            // Then, destroy
            Destroy(other.gameObject);
        }
    }
}
