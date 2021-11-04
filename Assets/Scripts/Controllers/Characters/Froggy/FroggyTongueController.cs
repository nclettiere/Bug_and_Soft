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
        private BaseController froggyController;
        private bool firstSlap;
        public bool isBoss;

        private void Start()
        {
            damagedObjects = new List<Transform>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
        {
            transform.localPosition = new Vector3(1, 0);
        }

        public void Anim_OnTongueAnimEnded()
        {
            //attackState.OnTongeFinished();
            froggyController.SendMessage("OnTongueFinish");
            Deactivate();
        }

        public void Anim_FirstSlapEnded()
        {
            firstSlap = true;
        }

        public void SetProps(BaseController controller)//, Froggy_AttackState attackState)
        {
            froggyController = controller; 
            //this.attackState = attackState;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (firstSlap || !other.transform.CompareTag("Player")) return;
            
            if(!isBoss)
                damageInfo = new DamageInfo(5, froggyController.transform.position.x, true, false, true);
            else
                damageInfo = new DamageInfo(15, froggyController.transform.position.x, true, false, true);
            damageInfo.MoveOnAttackForce = new Vector2(1000f, 500f);
            damageInfo.slowDuration = 3f;
                    
            var pctrl = other.transform.GetComponent<PlayerController>();
            if (pctrl != null)
                pctrl.Damage(froggyController, damageInfo);
                    
            firstSlap = true;
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
            firstSlap = false;
            gameObject.SetActive(false);
        }
    }
}