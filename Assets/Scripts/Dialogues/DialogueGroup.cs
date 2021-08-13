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

    public class DialogueGroup : MonoBehaviour
    {
        public LocalizedString[] locales;
        public DIALOGUE_ACTION[] dialogueActions;
        public UnityEvent dialogueActionCallbacks;
        public int from, to;
        public int current = -1;

        private void Awake()
        {
            if (dialogueActionCallbacks == null)
                dialogueActionCallbacks = new UnityEvent();
        }

        public int GetDialogueCount()
        {
            return (to - from);
        }

        public DialogueData GetDialogueLocale(int index)
        {
            if (index < locales.Length && index < dialogueActions.Length)
            {
                if (index == to - 1)
                    return new DialogueData(locales[index], dialogueActions[index], FinishAction);

                return new DialogueData(locales[index], dialogueActions[index], TestAction);
            }

            return new DialogueData(null, DIALOGUE_ACTION.NEXT, null);
        }

        public DialogueData NextDialogue()
        {
            current++;
            return GetDialogueLocale(current);
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
    }
}