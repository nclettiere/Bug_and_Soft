using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Items
{
    /// <summary>
    ///     Clase : Item : Antorcha
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Feature</term>
    ///             <description>Descripcion</description>
    ///         </listheader>
    ///         <item>
    ///             <term>Dinamica</term>
    ///             <description>Tanto el Player como los NPCs/Enemigos/Objetos pueden poseer esta camara.</description>
    ///         </item>
    ///         <item>
    ///             <term>Responsible</term>
    ///             <description>Puede ser accionada a traves del input o automaticamente.</description>
    ///         </item>
    ///         <item>
    ///             <term>Customizable</term>
    ///             <description>Se puede modificar la cantidad de smoothing, la velocidad, lockear axis.</description>
    ///         </item>
    ///     </list>
    /// </summary>
    /// <remarks>
    ///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
    /// </remarks>
    public class TorchComponent : 
    /// @cond SKIP_THIS
        MonoBehaviour
    /// @endcond
    {
        private bool lighten = false;
        [SerializeField] private Light2D torchFire;

        private bool routineRunning;

        AudioSource audioData;

        private void Start() 
        {
            audioData = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(lighten && !routineRunning)
            {
                StartCoroutine(UpdateLightIntensity(Random.Range(1f, 1.5f)));
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player") && torchFire != null && !lighten) 
            {
                lighten = true;
                audioData.Play();
                StartCoroutine(UpdateLightIntensity(1f));
            }
        }

        private IEnumerator UpdateLightIntensity(float endValue, float duration = 0.3f)
        {
            float time = 0;
            float startValue = torchFire.intensity;

            routineRunning = true;
            while (time < duration)
            {
                torchFire.intensity =
                    Mathf.Lerp(startValue, endValue, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            torchFire.intensity = endValue;
            routineRunning = false;
        }
    }
}