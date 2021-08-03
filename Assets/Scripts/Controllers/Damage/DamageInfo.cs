namespace Controllers.Damage
{
    /// <summary>
    /// Struct que contiene la informacion de un ataque.
    /// </summary>
    public struct DamageInfo
    {
        private int damageAmount;
        private float attackXPosition;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="damageAmount">La cantidad de danio a inflingir.</param>
        /// <param name="attackXPosition">La posicion X del ataque.</param>
        public DamageInfo(int damageAmount, float attackXPosition) : this()
        {
            DamageAmount = damageAmount;
            this.AttackXPosition = attackXPosition;
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
    }
}