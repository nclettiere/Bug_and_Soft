using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "AttackStateData", menuName = "States Data/State Data/Attack State Data", order = 2)]
    public class AttackStateData : ScriptableObject
    {
        public float duration = 1f;
        public float damage = 10f;
    }
}