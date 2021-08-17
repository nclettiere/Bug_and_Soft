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
        ///  Metodo utilizado para inflingir danio utilizando el struct DamageInfo.
        /// </summary>
        /// <param name="controller">Controlador del que inflinge damage.</param>
        /// <param name="damageInfo">La informacion del damage.</param>
        void Damage(BaseController controller, DamageInfo damageInfo);
        
        void Damage(DamageInfo damageInfo);
    }
}