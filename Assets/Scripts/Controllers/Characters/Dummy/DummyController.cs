using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///     <para>Controller para el enemigo Dummy. Controla el ciclo de vida, estado del personaje y sus animaciones.</para>
    /// </summary>
    /// <remarks>
    ///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
    /// </remarks>
    public class DummyController : BaseController
    {
        protected override void Start()
        {
            base.Start();
            
            controllerKind = EControllerKind.Enemy;
        }
    }
}