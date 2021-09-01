using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Dialogues
{
    public class DialogueManager
    {
        private static DialogueManager instance;
        private Canvas dialogueCanvas;
        private DialogueTextEffector dialogueEffector;

        private DialogueManager()
        {
            dialogueCanvas = GameObject.Find("/UI/UI_Dialogues").GetComponent<Canvas>();
            dialogueEffector = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<DialogueTextEffector>();
        }

        public static DialogueManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DialogueManager();

                return instance;
            }
        }

        public void ShowDialogues(DialogueGroup dialogues)
        {
            GameManager.PauseGame();
            dialogueCanvas.enabled = true;
            dialogueEffector.SetDialogues(dialogues);
            dialogueEffector.DisplayDialogues();
        }

        public void HideDialogues()
        {
            dialogueCanvas.enabled = false;
        }

        public void ChooseOption(int i)
        {
            dialogueEffector.ChooseOption(i);
        }
    }
}