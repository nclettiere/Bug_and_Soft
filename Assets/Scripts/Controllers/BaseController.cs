using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers.Movement;

namespace Controllers
{
    public class BaseController : MonoBehaviour
    {
        #region UserVariables
        [SerializeField]
        protected EControllerKind controllerKind = EControllerKind.NPC;
        public float GeneralSpeed = 40f;
        #endregion

        #region UserVariables
        protected BaseMovementController baseMovementController;
        protected Animator characterAnimator;

        protected ECharacterKind  characterKind;
        #endregion

        protected virtual void OnStart()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        protected virtual void OnUpdate()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        protected virtual void OnFixedUpdate()
        {
            // Debe ser llenado en una clase heredada
            // Ej: DummyController.cs
        }

        /// <summary>
        /// Obtiene el controller de moviemento del personaje especificando que tipo de moviemnto usa en T
        /// </summary>
        /// <typeparam name="T">Clase derivada de BaseMovementController</typeparam>
        /// <returns>Un objeto de clase T que hereda de BaseMovementController almacenado en el BaseController.</returns>
        public T GetMovementController<T>() where T : BaseMovementController
        {
            return (T) baseMovementController;
        }

        private void Start()
        {
            baseMovementController = GetComponent<BaseMovementController>();
            characterAnimator = GetComponent<Animator>();
            OnStart();
        }

        private void Update() {
            OnUpdate();
        }

        private void FixedUpdate() {
            OnFixedUpdate();
        }
    }
}
