using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Controllers.Froggy
{
    public class FroggyController : BaseController
    {
        protected override void OnStart()
        {
            controllerKind = EControllerKind.Enemy;
            characterKind = ECharacterKind.Froggy;

            InvokeRepeating("MoveCejas", 0f, 7f);
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
