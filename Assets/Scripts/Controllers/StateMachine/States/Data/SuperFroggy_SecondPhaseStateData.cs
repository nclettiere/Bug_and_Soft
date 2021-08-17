using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "SuperFroggy_SecondPhaseStateData", menuName = "States Data/State Data/SuperFroggy Second Phase State Data", order = 2)]
    public class SuperFroggy_SecondPhaseStateData : ScriptableObject
    {
        public GameObject bombs;
        public float bombCooldown = 1f;
    }
}