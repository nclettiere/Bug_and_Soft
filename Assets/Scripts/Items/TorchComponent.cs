using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Items
{
    /// <summary>
    ///     Script para las antorchas
    /// </summary>
    /// <remarks>
    ///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
    /// </remarks>
    public class TorchComponent : 
    /// @cond SKIP_THIS
        MonoBehaviour
    /// @endcond
    {
        [SerializeField]
        private bool lighten = false;
        [SerializeField] 
        private Light2D torchFire;

        private bool routineRunning;
        private AudioSource audioData;
        private Animator anim;

        private void Start() 
        {
            anim = GetComponent<Animator>();
            audioData = GetComponent<AudioSource>();
            anim.SetBool("Lighten", lighten);
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
                anim.SetBool("Lighten", true);
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