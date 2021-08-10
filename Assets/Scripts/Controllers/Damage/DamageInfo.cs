using UnityEngine;

namespace Controllers.Damage
{
    /// <summary>
    /// Struct que contiene la informacion de un ataque.
    /// </summary>
    public struct DamageInfo
    {
        private int damageAmount;
        private float attackXPosition;
        private bool moveOnAttack, stun, slow;
        public float stunDuration, slowDuration;
        private Vector2 moveOnAttackForce;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="damageAmount">La cantidad de danio a inflingir.</param>
        /// <param name="attackXPosition">La posicion X del ataque.</param>
        public DamageInfo(int damageAmount, float attackXPosition, bool moveOnAttack, bool stun, bool slow) : this()
        {
            DamageAmount = damageAmount;
            AttackXPosition = attackXPosition;
            MoveOnAttack = moveOnAttack;
            Stun = stun;
            Slow = slow;

            MoveOnAttackForce = new Vector2(10f, 10f);
        }

        /// <summary>
        /// Obtiene la direccion del ataque
        /// </summary>
        /// <param name="x">La posicion X del que recibe el ataque.</param>
        /// <returns>Retorna: 1 si es atacado desde la izquierda. y -1 por la derecha.</returns>
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