using System;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class SoundController : MonoBehaviour
    {
        public AudioSource[] ambientMusic;
        private int currentAmbientMusic;
        private int nextAmbientMusic = 1;
        
        private void Update()
        {
            PlayAmbientMusic();

            foreach (AudioSource source in ambientMusic)
            {
                source.volume = GameManager.Instance.MasterVolume;
            }
        }
        
        public void PlayAmbientMusic()
        {
            if (ambientMusic.Length > 0)
            {
                if (ambientMusic.Length <= currentAmbientMusic)
                {
                    if (!ambientMusic[currentAmbientMusic].isPlaying)
                    {
                        Debug.Log("Playinggg");
                        ambientMusic[currentAmbientMusic].Play();
                        currentAmbientMusic++;
                        nextAmbientMusic++;
                    }
                }
                else
                    currentAmbientMusic = 0;
            }
        }
    }
}