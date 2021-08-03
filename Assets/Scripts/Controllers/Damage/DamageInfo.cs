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
        public DamageInfo(int damageAmount, float attackXPosition)
        {
            this.damageAmount = damageAmount;
            this.attackXPosition = attackXPosition;
        }
    }
}