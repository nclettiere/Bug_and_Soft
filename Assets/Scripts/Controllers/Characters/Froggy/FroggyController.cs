using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Controllers.Froggy
{
    public class FroggyController : BaseController
    {
        [Header("Froggy Specific Values")]
        [SerializeField] private float jumpCooldownTime = 4f;
        private float lastJumpTime = float.NegativeInfinity;
        
        protected override void OnStart()
        {
            controllerKind = EControllerKind.Enemy;

            SwitchStates(ECharacterState.Jumping);

            InvokeRepeating("MoveCejas", 0f, 7f);
        }

        protected override bool OnUpdateJumpingStateStart()
        {
            return (Time.time >= lastJumpTime);
        }

        protected override void OnUpdateJumpingStateEnd()
        {
            lastJumpTime = Time.time + jumpCooldownTime;
        }

        /// <summary>
        ///     Metodo para empezar la animacion de mover cejas del PJ Njord
        /// </summary>
        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }

        /// <summary>
        ///     Metodo para terminar la animacion de mover cejas del PJ Njord
        /// </summary>
        /// <remarks>
        ///     Es llamado al final de la animacion cejas.
        /// </remarks>
        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }
    }
}
