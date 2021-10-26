using System;
using UnityEngine;

namespace World
{
    public class SpawnPoint : MonoBehaviour
    {
        private Animator _animator;
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                _animator.SetBool("Unlocked", true);
                GameManager.Instance.SetSpawnPoint(transform.position);
            }
        }
    }
}