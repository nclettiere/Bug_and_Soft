using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchComponent : MonoBehaviour
{
    [SerializeField] private bool lighten;
    [SerializeField] private Light2D torchFire;

    AudioSource audioData;

    private void Start() 
    {
        audioData = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && torchFire != null && !lighten) 
        {
            lighten = true;
            audioData.Play(0);
            StartCoroutine(UpdateLightIntensity(1f));
        }
    }

    private IEnumerator UpdateLightIntensity(float endValue, float duration = 0.3f)
    {
        float time = 0;
        float startValue = torchFire.intensity;

        while (time < duration)
        {
            torchFire.intensity =
                Mathf.Lerp(startValue, endValue, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
        torchFire.intensity = endValue;
    }
}
