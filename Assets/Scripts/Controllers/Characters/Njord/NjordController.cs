using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class NjordController : BaseController
    {
        protected override void OnStart()
        {
            controllerKind = EControllerKind.Neutral;

            InvokeRepeating("MoveCejas", 0f, 5f);
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