using System.Collections;
using System.Collections.Generic;
using Controllers.Damage;
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

        /// <summary>
        ///  Metodo utilizado para inflingir danio utilizando el struct DamageInfo.
        /// </summary>
        /// <param name="controller">Controlador del que inflinge damage.</param>
        /// <param name="damageInfo">La informacion del damage.</param>
        void Damage(BaseController controller, DamageInfo damageInfo);

        /// <summary>
        ///     Metodo utilizado para matar a un personaje
        /// </summary>
        void Kill();
    }
}