using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "DamageStateData", menuName = "States Data/State Data/Damage State Data", order = 2)]
    public class DamageStateData : ScriptableObject
    {
        public float duration = 1f;
    }
}