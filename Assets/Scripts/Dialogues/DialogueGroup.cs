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

    public class   DialogueGroup : MonoBehaviour
    {
        public Dialogue[] dialogues;
        public uint SelectedDialogue;
        public UnityEvent dialogueActionCallbacks;

        private void Awake()
        {
            if (dialogueActionCallbacks == null)
                dialogueActionCallbacks = new UnityEvent();
        }

        private void TestAction()
        {
            DialogueManager.Instance.ChooseOption(2);
        }

        private void FinishAction()
        {
            DialogueManager.Instance.HideDialogues();
            GameManager.Instance.PlayerExitInteractionMode();
        }

        public void Run()
        {
            Debug.Log("Is null? "+ dialogues[SelectedDialogue] == null);
            dialogues[SelectedDialogue].ShowDialogues();
        }
    }
}