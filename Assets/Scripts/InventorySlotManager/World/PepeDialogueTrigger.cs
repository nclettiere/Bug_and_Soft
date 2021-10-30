﻿using System;
using Controllers;
using UnityEngine;

namespace World
{
    public class PepeDialogueTrigger : MonoBehaviour
    {
        [SerializeField] private PepeController pepeController;
        [SerializeField] private Sprite dialogueText;
        private bool _textShown;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (!_textShown)
                {
                    pepeController.ShowQuickChat(new Tuple<Sprite, int>(dialogueText, 3));
                    _textShown = true;
                }
            }
        }
    }
}