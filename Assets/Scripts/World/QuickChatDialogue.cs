using System;
using System.Collections;
using UnityEngine;

namespace World
{
    public class QuickChatDialogue : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        
        private static readonly int Trigger = Animator.StringToHash("Trigger");

        public void ShowNewChat(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;

            gameObject.SetActive(true);
            StartCoroutine(StartQuickChat());
        }

        private IEnumerator StartQuickChat()
        {
            animator.SetBool(Trigger, true);
            yield return new WaitForSeconds(3f);
            animator.SetBool(Trigger, false);
        }

        public void OnAnimEnd()
        {
            gameObject.SetActive(false);
        }

        public void Flip()
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}