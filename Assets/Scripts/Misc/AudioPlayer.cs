using UnityEngine;

namespace Misc
{
    public class AudioPlayer : MonoBehaviour
    {
        public void Play(AudioSource audio)
        {
            audio.Play();
        }
    }
}