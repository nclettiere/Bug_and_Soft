using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Controllers.StateMachine.States.Data
{
    [CreateAssetMenu(fileName = "JumpStateData", menuName = "States Data/State Data/Jump State Data", order = 0)]
    public class JumpStateData : ScriptableObject
    {
        public Vector2 jumpingForce = new Vector2(3f, 5f);
        //public float   torqueForce = 10f;
        public float jumpingCooldownRandomRangeFrom = 2.5f;
        public float jumpingCooldownRandomRangeTo = 5f;
        public AudioClip jumpSFX;
        public AudioClip landSFX;
    }
}