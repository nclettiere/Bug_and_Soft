using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "PrepareAttackStateData", menuName = "States Data/State Data/Prepare Attack State Data", order = 2)]
    public class PrepareAttackStateData : ScriptableObject
    {
        public bool followPlayerDirection = true;
        public float cooldown = 1f;
    }
}