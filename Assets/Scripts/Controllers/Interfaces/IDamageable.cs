using System.Collections;
using System.Collections.Generic;
using Controllers.Damage;
using UnityEngine;

namespace Controllers
{
    public interface IDamageable
    {
        void Damage(BaseController controller, DamageInfo damageInfo);
        
        void Damage(DamageInfo damageInfo);
    }
}