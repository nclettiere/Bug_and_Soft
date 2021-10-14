using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands;
using Controllers.Damage;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Froggy
{
    public class FroggyTongueController : MonoBehaviour
    {
        private Froggy_AttackState attackState;
        private BoxCollider2D coll;

        private List<Transform> damagedObjects;
        private DamageInfo damageInfo;
        private FroggyController froggyController;
        private bool firstSlap;

        private void Start()
        {
            damagedObjects = new List<Transform>();
            coll = GetComponent<BoxCollider2D>();
            damageInfo = new DamageInfo(15, transform.position.x, true, false, true);
            damageInfo.MoveOnAttackForce = new Vector2(1000f, 500f);
            damageInfo.slowDuration = 3f;
        }

        private void FixedUpdate()
        {
            // Copia la posicion del Froggy
            transform.localPosition = new Vector3(1, 0);
            //CheckDamage();
        }

        public void Anim_OnTongueAnimEnded()
        {
            attackState.OnTongeFinished();
            Deactivate();
        }

        public void Anim_FirstSlapEnded()
        {
            firstSlap = true;
        }

        public void SetProps(FroggyController froggyController, Froggy_AttackState attackState)
        {
            this.froggyController = froggyController;
            this.attackState = attackState;
        }

        private void CheckDamage()
        {
            if (!firstSlap)
            {
                //RaycastHit2D[] hits = new RaycastHit2D[10];
                //coll.Cast(transform.right, whatCanBeDamaged, hits);
                //foreach (var hit in hits)
                //{
                //    if (hit.collider != null)
                //    {
                //        if (hit.collider.transform != froggyController.transform &&
                //            !damagedObjects.Contains(hit.collider.transform))
                //        {
                //            if (hit.collider.transform.CompareTag("Player"))
                //            {
                //                PlayerController pctrl = hit.transform.GetComponent<PlayerController>();
                //                if (pctrl != null && (pctrl is IDamageable))
                //                    pctrl.Damage(froggyController, damageInfo);
                //            }
//
                //            damagedObjects.Add(hit.collider.transform);
                //        }
                //    }
                //}
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                Debug.Log("Player is colliding");
                var pctrl = other.transform.GetComponent<PlayerController>();
                if (pctrl != null)
                    pctrl.Damage(froggyController, damageInfo);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                Debug.Log("Player is colliding 2");
                var pctrl = other.transform.GetComponent<PlayerController>();
                if (pctrl != null)
                    pctrl.Damage(froggyController, damageInfo);
            }
        }

        public void Cancel()
        {
            Deactivate();
        }

        public void Activate()
        {
            
        }
        
        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}