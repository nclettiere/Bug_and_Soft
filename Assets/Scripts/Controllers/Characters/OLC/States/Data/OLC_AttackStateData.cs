using UnityEngine;

namespace Controllers.Froggy.States.Data
{
    [CreateAssetMenu(fileName = "OLC_AttackStateData", menuName = "States Data/State Data/OLC_AttackStateData", order = 0)]
    public class OLC_AttackStateData : ScriptableObject
    {
        public GameObject projectile;
    }
}