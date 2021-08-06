using System;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Froggy
{
    public class FroggyTongueController : MonoBehaviour
    {
        public bool exitAnim;
        private Froggy_AttackState attackState;
        private FroggyController froggyController;

        private void Update()
        {
            transform.position = froggyController.transform.position;
        }

        public void Anim_OnTongueAnimEnded()
        {
            exitAnim = true;
            attackState.OnTongeFinished();
            Destroy(gameObject);
        }

        public void SetProps(FroggyController froggyController, Froggy_AttackState attackState)
        {
            this.froggyController = froggyController;
            this.attackState = attackState;
        }
    }
}