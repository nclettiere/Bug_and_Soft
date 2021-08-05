using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "ControllerData", menuName = "States Data/Controller Data/Base Data", order = 0)]
    public class ControllerData : ScriptableObject
    {
        [Header("World Checks")]
        public float wallCheckDistance = 0.2f;
        public float ledgeCheckDistacne = 0.5f;
        public float groundCheckDistance = 0.5f;
        public float topCheckDistance = 0.5f;
        public float playerDetectionDistance = 10f;
        public LayerMask whatIsGround, whatIsPlayer;
    }
}