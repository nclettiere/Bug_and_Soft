using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class DynamicCamera : MonoBehaviour
    {   
        // Fields for new camera controller : Propiedades para la nueva camara
        [Header("(Required) Player or Object to look at")]
        [SerializeField] private Transform target; // el target puede ser un jugador/NPC/objeto; totalmente dinamico.
        
        [Header("(Optional) Camera options")]
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 3.5f, -1f);
        [SerializeField] private bool allowBounds = true; // Limites de la camara (No sigue al jugador hasta que haya pasado cierto punto)
        [SerializeField] private bool allowSmoothing = true; // Permite o no el suavizado de la camara; ***SMOTHIEEEEE LET'S GOOOOOOOO***
        [SerializeField] private float bounds = 3f; // Distancia a la que empieza moverse la camara.
        [Range(0, 10)] [SerializeField] private float smoothingAmount = 10f; // En un rango de (0 - 10), especifica la cantidad de suavizado de la camara.
        
        private Vector3 newCameraPosition; // La posicion a la que se movera la camara;

        /// <summary>
        /// Utiliza FixedUpdate para esperar al movimiento del jugador/NPC/objeto.
        /// Define la variable newCameraPosition y si es necesario, agrega los extras (bound limit y smoothing)
        /// </summary>
        private void FixedUpdate() 
        {
            if (target == null)
                return;

            // Base position of the camera : posicion inicial de la camara
            newCameraPosition = target.position + cameraOffset;

            // [Extras]

            // [Section][Bounds]
            if(allowBounds)
            {
                // Checkea si se esta dentro de los limites o no.

                // deltaX de la posicion de la camara y el target
                float deltaX =  target.position.x - transform.position.x;
                
                // if(camara dentro de limites)
                if(Mathf.Abs(deltaX) > bounds)
                {
                    // (dX > 0) ==> A la derecha de la pantalla
                    if(deltaX > 0) 
                    {
                        // Alinea la pos. de la camara hacia la derecha de la pantalla
                        newCameraPosition.x = target.position.x - bounds;
                    }else {
                        // Izquierda
                        newCameraPosition.x = target.position.x + bounds;
                    }
                }else 
                {
                    newCameraPosition.x = target.position.x - deltaX;
                }
            }
            // [End][Section][Bounds]

            // [Section][Smoothing]
            if(allowSmoothing)
                transform.position = Vector3.Lerp(transform.position, newCameraPosition, Time.deltaTime * smoothingAmount);
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
    }
}