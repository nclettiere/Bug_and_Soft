using System;
using System.Collections;
using Controllers.Damage;
using Player;
using UnityEngine;

namespace World
{
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private int _damageAmount = 20;
        [SerializeField] private LayerMask whatIsPlayer;

        private Animator _animator;
        private DamageInfo _damageInfo;

        private bool shot;
        
        public bool IsShooting { get; private set; }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _damageInfo = new DamageInfo(_damageAmount, transform.position.x, true);
        }

        public void Shoot()
        {
            StartCoroutine(Shooter());
        }

        public void Anim_Shot()
        {
            shot = true;
        }

        public void Anim_BeamEnded()
        {
            IsShooting = false;
            _animator.SetBool("Shoot", false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                other.transform.GetComponent<PlayerController>()
                    .Damage(_damageInfo);
            }
        }

        private IEnumerator Shooter()
        {
            shot = false;
            IsShooting = true;
            _animator.SetBool("Shoot", true);
            yield return new WaitForSeconds(1);
            IsShooting = false;
            _animator.SetBool("Shoot", false);
        }
    }
}