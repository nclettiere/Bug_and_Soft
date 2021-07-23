using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///     Interfaz para todos los NPCs/Enemigos/Objetos que pueden ser daniados.
    /// </summary>
    /// <remarks>
    ///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
    /// </remarks>
    public interface IDamageable
    {
        /// <summary>
        ///     Metodo utilizado para inflingir danio a cualquiera que implemente este metodo.
        /// </summary>
        /// <param name="amount">La cantidad de danio a inflingir</param>
        /// <param name="attackN">Numero del ataque en el combo.</param>
        void Damage(float amount, int attackN);
    }
}