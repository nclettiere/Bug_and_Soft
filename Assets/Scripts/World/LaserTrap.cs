using System;
using System.Collections;
using Controllers.Damage;
using UnityEngine;

namespace World
{
    [ExecuteInEditMode]
    public class LaserTrap : MonoBehaviour
    {
        [SerializeField] private float _actuationSize;
        [SerializeField] private int _damageAmount = 20;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private BoxCollider2D _boxCollider2D;
        [SerializeField] private LaserBeam _laserBeam;

        private Animator _animator;
        private DamageInfo _damageInfo;
        private bool _playerInRange;
        private static readonly int PlayerInRange = Animator.StringToHash("PlayerInRange");
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _damageInfo = new DamageInfo(_damageAmount, transform.position.x, true);
        }

        private void Update()
        {
            _boxCollider2D.offset = new Vector2((_actuationSize / 2) * -1, 0);
            _boxCollider2D.size = new Vector2(_actuationSize, 1);

            ActuationCheck();
        }

        private void ActuationCheck()
        {
            //if(!_laserBeam.IsShooting)
            //    _animator.SetBool(PlayerInRange, _playerInRange);
        }

        public void Anim_ShootNow()
        {
            StartCoroutine(Shooter());
        }

        private void ShootCheck()
        {
           
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
                _animator.SetBool(PlayerInRange, true);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
                _animator.SetBool(PlayerInRange, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
                _animator.SetBool(PlayerInRange, false);
        }

        private IEnumerator Shooter()
        {
            _laserBeam.Shoot();
            _animator.SetBool(PlayerInRange, false);
            _animator.SetBool(Shoot, true);
            yield return new WaitWhile(() => _laserBeam.IsShooting);
            Debug.Log("_laserBeam.IsShooting is false!");
            yield return new WaitForSeconds(1);
            _animator.SetBool(Shoot, false);
        }
    }
}