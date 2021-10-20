using System.Collections;
using UnityEngine;

namespace World
{
    public class EnemyTauntIndicator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private bool _isInCooldown;

        private bool _isShowing;
        private static readonly int Activate = Animator.StringToHash("Activate");
        private static readonly int Activated = Animator.StringToHash("Activated");

        public void Show()
        {
            if (!_isShowing && !_isInCooldown)
                StartCoroutine(ShowIndicator());
            
        }

        private IEnumerator ShowIndicator()
        {
            animator.SetBool(Activate, true);
            _isShowing = true;
            _isInCooldown = true;
            yield return new WaitForSeconds(2);
            animator.SetBool(Activate, false);
            animator.SetBool(Activated, false);
            yield return new WaitForSeconds(7f);
            _isInCooldown = false;
            _isShowing = false;
            gameObject.SetActive(false);
        }

        public void Anim_TransitionDone()
        {
            animator.SetBool(Activate, false);
            animator.SetBool(Activated, true);
        }
    }
}