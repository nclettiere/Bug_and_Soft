using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewDialogues
{
    public class NewDialogueGroup : MonoBehaviour
    {
        [SerializeField] private NewDialogue[] dialogues;

        public ref NewDialogue[] GetDialogues()
        {
            return ref dialogues;
        }
    }
}