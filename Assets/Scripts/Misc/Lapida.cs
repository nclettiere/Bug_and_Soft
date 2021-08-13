using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    public class Lapida : MonoBehaviour
    {
        private Animator anim;
        private AudioSource lapidaOnSFX;
        private bool isLapidaOn = false;

        private void OnEnable()
        {
            anim = GetComponent<Animator>();
            lapidaOnSFX = GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(lapidaOnSFX.clip, transform.position);
        }

        private void LapidaAnimOnFinished()
        {
            isLapidaOn = true;
            StartCoroutine(StartDesttoyLapida());
        }
        
        private void LapidaAnimOffFinished()
        {
            Destroy(gameObject);
        }

        private IEnumerator StartDesttoyLapida()
        {
            yield return new WaitForSeconds(Random.Range(1f, 3.5f));
            AudioSource.PlayClipAtPoint(lapidaOnSFX.clip, transform.position);
            anim.SetBool("DestroyLapida", true);
        }
    }
}