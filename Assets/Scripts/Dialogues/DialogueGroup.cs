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
        private PerformAction handler;

        public DialogueData(LocalizedString locale, DIALOGUE_ACTION action, PerformAction performAction)
        {
            Locale = locale;
            Action = action;
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
        public int DialogueIndex;

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