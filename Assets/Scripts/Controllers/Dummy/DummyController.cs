using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class DummyController : BaseController
    {
        protected override void OnStart()
        {
            controllerKind = EControllerKind.Enemy;
            characterKind = ECharacterKind.Dummy;
        }
    }
}