using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class DialogueChoices
    {
        [SerializeField] public EDialogueChoice[] Choices;
    }
}