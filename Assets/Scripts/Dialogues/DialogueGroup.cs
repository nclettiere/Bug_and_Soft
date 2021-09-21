using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Dialogues
{
    public enum DIALOGUE_ACTION
    {
        NEXT,
        CHOICE,
        FINISH
    }


    public delegate void PerformAction();

    public struct DialogueData
    {
        public LocalizedString Locale;
        public DIALOGUE_ACTION Action;
        public EDialogueChoice[] DialogueChoices;
        private PerformAction handler;

        public DialogueData(LocalizedString locale, DIALOGUE_ACTION action, EDialogueChoice[] dialogueChoices, PerformAction performAction)
        {
            Locale = locale;
            Action = action;
            DialogueChoices = dialogueChoices;
            handler = performAction;
        }

        public void RunAction()
        {
            handler();
        }
    }

    public class  DialogueGroup : MonoBehaviour
    {
        public Dialogue[] dialogues;
        public int DialogueIndex = 2;

        public void Run()
        {
            dialogues[DialogueIndex].ShowDialogues();
        }
        
        public void SetDialogueIndex(int i)
        {
            Debug.Log("Dialogue index setted to: " + i);
            DialogueIndex = i;
        }
    }
    
}