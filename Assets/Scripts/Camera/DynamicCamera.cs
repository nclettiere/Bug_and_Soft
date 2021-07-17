using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManagement
{
    public class DynamicCamera : MonoBehaviour
    {
        [Header("(Required) Camera object and target")]

        [SerializeField]
        private Camera CameraObject; // el target puede ser un jugador/NPC/objeto; totalmente dinamico.
        [SerializeField]
        private Transform target; // el target puede ser un jugador/NPC/objeto; totalmente dinamico.

        [Header("(Optional) Camera options")]
        [SerializeField]
        private Vector3 cameraOffset = new Vector3(10f, 3.5f, -1f);

        [SerializeField]
        private bool allowBounds = true; // Limites de la camara (No sigue al jugador hasta que haya pasado cierto punto)

        [SerializeField]
        private bool allowSmoothing = true; // Permite o no el suavizado de la camara; ***SMOTHIEEEEE LET'S GOOOOOOOO***

        [SerializeField]
        private float bounds = 3f; // Distancia a la que empieza moverse la camara.

        [Range(0, 10)]
        [SerializeField]
        private float smoothingAmount = 10f; // En un rango de (0 - 10), especifica la cantidad de suavizado de la camara.

        private Vector3 newCameraPosition; // La posicion a la que se movera la camara;

        /// <summary>
        /// Utiliza FixedUpdate para esperar al movimiento del jugador/NPC/objeto.
        /// Define la variable newCameraPosition y si es necesario, agrega los extras (bound limit y smoothing)
        /// </summary>
        private void FixedUpdate()
        {
            if (target == null) return;

            // Base position of the camera : posicion inicial de la camara
            newCameraPosition = target.position + cameraOffset;

            // [Extras]
            // [Section][Bounds]
            if (allowBounds)
            {
                // Checkea si se esta dentro de los limites o no.
                // deltaX de la posicion de la camara y el target
                float deltaX = target.position.x - transform.position.x;

                // if(camara dentro de limites)
                if (Mathf.Abs(deltaX) > bounds)
                {
                    // (dX > 0) ==> A la derecha de la pantalla
                    if (deltaX > 0)
                    {
                        // Alinea la pos. de la camara hacia la derecha de la pantalla
                        newCameraPosition.x =
                            target.position.x - bounds + cameraOffset.x;
                    }
                    else
                    {
                        // Izquierda
                        newCameraPosition.x =
                            target.position.x + bounds + cameraOffset.x;
                    }
                }
                else
                {
                    newCameraPosition.x =
                        target.position.x - deltaX + cameraOffset.x;      
                }
            }

            // [End][Section][Bounds]
            // [Section][Smoothing]
            if (allowSmoothing)
                transform.position =
                    Vector3
                        .Lerp(transform.position,
                        newCameraPosition,
                        Time.deltaTime * smoothingAmount);
            else
                transform.position = newCameraPosition;
            // [End][Section][Smoothing]
        }

        /// <summary>
        /// Metodo para cambiar el target de la camara
        /// </summary>
        /// <param name="target">Un Transform de cualquier objeto instanciado en el juego.</param>
        private void ChangeTarget(Transform target)
        {
            this.target = target;
        }

        /// <summary>
        /// Metodo para actualizar el offset de la camara
        /// </summary>
        internal void UpdateOffset(Vector2 offset)
        {
            cameraOffset = new Vector3(offset.x, offset.y, -1f);
        }

        /// <summary>
        /// Metodo para actualizar el offset X de la camara
        /// </summary>
        internal void UpdateOffsetX(float offsetX)
        {
            cameraOffset.x = offsetX;
        }

        /// <summary>
        /// Metodo para actualizar el offset Y de la camara
        /// </summary>
        internal void UpdateOffsetY(float offsetY)
        {
            cameraOffset.y = offsetY;
        }

        /// <summary>
        /// Metodo para actualizar el size del lente de la camara.
        /// </summary>
        internal void UpdateSize(float size, float duration = 3f)
        {
            if(CameraObject != null)
                StartCoroutine(UpdateCameraSize(size, duration));
        }

        private IEnumerator UpdateCameraSize(float endValue, float duration)
        {
            float time = 0;
            float startValue = CameraObject.orthographicSize;

            while (time < duration)
            {
                CameraObject.orthographicSize =
                    Mathf.Lerp(startValue, endValue, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            CameraObject.orthographicSize = endValue;
        }

        private IEnumerator UpdateCameraOffsetX(float endValue, float duration = 0.3f)
        {
            float time = 0;
            float startValue = cameraOffset.x;

            while (time < duration)
            {
                cameraOffset.x =
                    Mathf.Lerp(startValue, endValue, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            cameraOffset.x = endValue;
        }
    }
}
