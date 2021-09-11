using System.Collections;
using System.Collections.Generic;
using Controllers.Damage;
using UnityEngine;

namespace Controllers
{
    public interface IDamageable
    {
        /// <deprecated>
        /// Funcion desactualizada. Se le RECOMIENDA utilizar Damage(DamageInfo damageInfo) instead.
        /// </deprecated>
        void Damage(BaseController controller, DamageInfo damageInfo);
        
        void Damage(DamageInfo damageInfo);
    }
}