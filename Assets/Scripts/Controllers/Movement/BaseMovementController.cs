using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Movement
{
    public class BaseMovementController :
        MonoBehaviour
    {
        public float Speed = 40f;
        
        protected Rigidbody2D rBody;

        private Vector2 velocity = new Vector2(0f, 0f);
        private BaseController bCtrl;
        
        protected virtual void Start()
        {
            rBody = GetComponent<Rigidbody2D>();
            bCtrl = GetComponent<BaseController>();
        }
        
        public virtual void Move(int direction, bool applyBoost)
        {
            velocity.Set(Speed * direction, rBody.velocity.y);
            // TODO: ApplyBoost
            rBody.velocity = velocity;
        }
        
        public virtual void Move()
        {
            velocity.Set(Speed * bCtrl.FacingDirection, rBody.velocity.y);
            // TODO: ApplyBoost
            rBody.velocity = velocity;
        }
    }
}