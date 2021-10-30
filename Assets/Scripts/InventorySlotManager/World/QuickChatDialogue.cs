using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class QuickChatDialogue : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        
        private bool _displayingQuickChat;
        private int _waitTime;
        private Queue<Tuple<Sprite, int>> _quickChats;
        
        private static readonly int Trigger = Animator.StringToHash("Trigger");

        private void Awake()
        {
            _quickChats = new Queue<Tuple<Sprite, int>>();
        }

        public void ShowNewChat(Tuple<Sprite, int> quickChat)
        {
            _quickChats.Enqueue(quickChat);
        }

        private void Update()
        {
            if (_quickChats.Count > 0)
            {
                if(!_displayingQuickChat)
                {
                    _displayingQuickChat = true;
                    var quickChat = _quickChats.Dequeue();
                    spriteRenderer.sprite = quickChat.Item1;
                    _waitTime = quickChat.Item2;
                    animator.SetBool(Trigger, true);
                }
            }
        }

        public void OnQCShown()
        {
            StartCoroutine(QuickChatDelay());
        }
        
        public void OnQCEnd()
        {
            _displayingQuickChat = false;
        }

        private IEnumerator QuickChatDelay()
        {
            yield return new WaitForSeconds(_waitTime);
            animator.SetBool(Trigger, false);
            yield return 0;
        }

        public void Flip()
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}