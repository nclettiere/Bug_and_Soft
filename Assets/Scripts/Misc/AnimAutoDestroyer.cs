using UnityEngine;

namespace Misc
{
    public class AnimAutoDestroyer : MonoBehaviour
    {
        [SerializeField] private AudioSource onEndedAudio;

        public void PlayAudio()
        {
            GameManager.Instance.GetSoundManager().PlaySoundAtLocation(onEndedAudio.clip, transform.position);
        }
        
        public void OnAnimEnded()
        {
            Destroy(gameObject);
        }
    }
}