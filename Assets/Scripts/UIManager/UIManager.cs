using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UIManager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject blindEffectIndicator;
        [SerializeField] private GameObject poisonEffectIndicator;

        private bool isBlindnessActive;

        public void ApplyBlindness(float howLong)
        {
            if (!isBlindnessActive)
            {
                Volume volume = GetLevelPostProcess();
                Vignette vignette;
                volume.profile.TryGet(typeof(Vignette), out vignette);

                isBlindnessActive = true;

                StartCoroutine(BlindPlayerFor(vignette, howLong, 0f, 0.6f));
            }
        }

        private IEnumerator BlindPlayerFor(Vignette vignette, float seconds, float from, float to)
        {
            
            SetEffectActive(EEffectKind.BLINDNESS);
            
            float counter = 0f;
            while (counter < 0.4f)
            {
                counter += GameManager.Instance.DeltaTime;
                vignette.intensity.value = Mathf.Lerp(from, to, counter / 0.4f);

                yield return null;
            }

            yield return new WaitForSeconds(seconds);

            counter = 0f;

            
            DeactivateEffectIndicator(blindEffectIndicator);
            isBlindnessActive = false;
            
            while (counter < seconds)
            {
                counter += GameManager.Instance.DeltaTime;
                vignette.intensity.value = Mathf.Lerp(to, from, counter / 0.6f);

                yield return null;
            }
        }

        public void SetEffectActive(EEffectKind effectKind)
        {
            switch (effectKind)
            {
                case EEffectKind.BLINDNESS:
                    ActivateEffectIndicator(blindEffectIndicator, effectKind);
                    isBlindnessActive = true;
                    break;
            }
        }

        private void ActivateEffectIndicator(GameObject whatIndicator, EEffectKind effectKind)
        {
            whatIndicator.SetActive(true);
            //StartCoroutine(DeactivateEffectIndicator(whatIndicator, effectKind, howLong));
        }

        private IEnumerator DeactivateEffectIndicator(GameObject whatIndicator, EEffectKind effectKind, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            whatIndicator.SetActive(false);

            switch (effectKind)
            {
                case EEffectKind.BLINDNESS:
                    isBlindnessActive = false;
                    break;
            }
        }
        
        private void DeactivateEffectIndicator(GameObject whatIndicator)
        {
            whatIndicator.SetActive(false);
        }

        private Volume GetLevelPostProcess()
        {
            return GameObject.Find("PostProcess").GetComponent<Volume>();
        }
    }
}