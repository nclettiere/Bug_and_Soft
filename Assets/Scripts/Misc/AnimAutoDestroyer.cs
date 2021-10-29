using UnityEngine;

namespace Misc
{
    public class AnimAutoDestroyer : MonoBehaviour
    {
        [SerializeField] private AudioSource onEndedAudio;

        public void PlayAudio()
        {
            onEndedAudio.Play();
        }
        
        public void OnAnimEnded()
        {
            Destroy(gameObject);
        }
    }
}