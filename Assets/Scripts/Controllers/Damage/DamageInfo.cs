using UnityEngine;

namespace Controllers.Damage
{
    public struct DamageInfo
    {
        private int damageAmount;
        private float attackXPosition;
        private bool moveOnAttack, stun, slow;
        public bool isLava;
        public float stunDuration, slowDuration;
        private Vector2 moveOnAttackForce;

        public DamageInfo(int damageAmount, float attackXPosition, bool moveOnAttack = false, bool stun = false, bool slow = false) : this()
        {
            DamageAmount = damageAmount;
            AttackXPosition = attackXPosition;
            MoveOnAttack = moveOnAttack;
            Stun = stun;
            Slow = slow;

            MoveOnAttackForce = new Vector2(10f, 10f);
        }

        public int GetAttackDirection(float x)
        {
            int direction = (AttackXPosition < x) ? direction = 1 : direction = -1;
            return direction;
        }

        public int DamageAmount
        {
            get => damageAmount;
            set => damageAmount = value;
        }

        public float AttackXPosition
        {
            get => attackXPosition;
            set => attackXPosition = value;
        }

        public bool MoveOnAttack
        {
            get => moveOnAttack;
            set => moveOnAttack = value;
        }

        public bool Stun
        {
            get => stun;
            set => stun = value;
        }

        public bool Slow
        {
            get => slow;
            set => slow = value;
        }

        public Vector2 MoveOnAttackForce
        {
            get => moveOnAttackForce;
            set => moveOnAttackForce = value;
        }
    }
}