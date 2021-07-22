using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public interface IDamageable
    {
        void Damage(float amount, int attackN);
    }
}