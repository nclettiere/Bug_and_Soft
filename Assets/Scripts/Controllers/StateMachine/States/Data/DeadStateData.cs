using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "DeadState", menuName = "States Data/State Data/Dead Data", order = 0)]
    public class DeadStateData : ScriptableObject
    {
        public bool applyTorque = true;
        public GameObject lapida;
        public bool showLapida = true;
    }
}