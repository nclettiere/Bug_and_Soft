using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "States Data/State Data/Idle Data", order = 0)]
    public class IdleStateData : ScriptableObject
    {
        public float minIdleTime = 1f;
        public float maxIdleTime = 4f;
    }
}