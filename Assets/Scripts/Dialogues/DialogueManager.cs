using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        public static string text = "";
        
        private static DialogueManager instance;
        private static Canvas dialogueCanvas;
        private static DialogueTextEffector dialogueEffector;
        private static LocalizeStringEvent localeStringEvent;
        public static TextMeshProUGUI textRenderer;

        public static DialogueManager Instance
        {
            get
            {
                
                if (instance == null)
                {
                    instance = GameObject.Find("Managers/DialogueManager").AddComponent<DialogueManager>();
                    FindObjects();
                }

                return instance;
            }
        }
        
        private static void FindObjects()
        {
            dialogueCanvas = GameObject.Find("/UI/UI_Dialogues").GetComponent<Canvas>();
            dialogueEffector = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<DialogueTextEffector>();
            localeStringEvent = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<LocalizeStringEvent>();
            textRenderer = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<TextMeshProUGUI>();
        }

        public void ShowDialogues(DialogueGroup dialogues)
        {
            GameManager.Instance.EnterDialogueMode();
            dialogueCanvas.enabled = true;
            dialogues.Run();
        }

        public void HideDialogues()
        {
            dialogueCanvas.enabled = false;
        }

        public void ChooseOption(int i)
        {
            dialogueEffector.ChooseOption(i);
        }

        public LocalizeStringEvent GetLocalizeStrEvent()
        {
            return localeStringEvent;
        }
        
        public static string Text
        {
            get => text;
            set
            {
                Debug.Log("Setting text to: "+ value);
                text = value;
            }
        }
    }
}