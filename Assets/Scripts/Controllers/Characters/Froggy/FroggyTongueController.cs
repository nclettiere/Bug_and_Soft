using System;
using System.Collections.Generic;
using System.Linq;
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
        private GameObject tongueEnd;
        public ContactFilter2D whatCanBeDamaged;

        private void Start()
        {
            tongueEnd = transform.Find("TongueEnd").gameObject;
            damagedObjects = new List<Transform>();
            coll = GetComponent<BoxCollider2D>();
            damageInfo = new DamageInfo(15, transform.position.x, true, false, true);
            damageInfo.MoveOnAttackForce = new Vector2(1000f, 500f);
            damageInfo.slowDuration = 3f;
        }

        private void FixedUpdate()
        {
            // Copia la posicion del Froggy
            transform.position = froggyController.transform.position;
            CheckDamage();
        }

        public void Anim_OnTongueAnimEnded()
        {
            attackState.OnTongeFinished();
            Destroy(gameObject);
        }

        public void SetProps(FroggyController froggyController, Froggy_AttackState attackState)
        {
            this.froggyController = froggyController;
            this.attackState = attackState;
        }

        private void CheckDamage()
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            coll.Cast(Vector2.right, whatCanBeDamaged, hits);
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform != froggyController.transform &&
                        !damagedObjects.Contains(hit.collider.transform))
                    {
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            BaseController bctrl = hit.transform.GetComponent<BaseController>();
                            if (bctrl != null && (bctrl is IDamageable))
                                bctrl.Damage(froggyController, damageInfo);
                        }
                        else if (hit.collider.CompareTag("Player"))
                        {
                            PlayerController pctrl = hit.transform.GetComponent<PlayerController>();
                            if (pctrl != null && (pctrl is IDamageable))
                                pctrl.Damage(froggyController, damageInfo);
                        }

                        damagedObjects.Add(hit.collider.transform);
                    }
                }
            }
        }
    }
}