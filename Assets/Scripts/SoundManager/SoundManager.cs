using System;
using UnityEngine;

namespace Scripts.SoundManager
{
    public class SoundManager : MonoBehaviour
    {
        [Range(0, 1)] public float MasterVolume = 0.3f;
        [SerializeField] private AudioSource AmbianceMusic;

        private void Start()
        {
            PlayAmbianceMusic(AmbianceMusic.clip, true);
        }

        public void PlaySoundAtLocation(AudioClip audioClip, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(audioClip, position);
        }

        public void PlayAmbianceMusic(AudioClip audioClip, bool loop)
        {
            AmbianceMusic.Stop();
            AmbianceMusic.clip = audioClip;
            AmbianceMusic.loop = loop;
            AmbianceMusic.Play();
        }

        public void ChangeMasterVolume(float target)
        {
            MasterVolume = target;
            AmbianceMusic.volume = MasterVolume;
        }
    }
}
//https://www.youtube.com/watch?v=ETPZMVHI0PY