using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        public static string text = "";
        
        private static DialogueManager instance;
        private static Canvas dialogueCanvas;
        private static Button interactButton;
        private static TextMeshProUGUI interactButtonText;
        private static DialogueTextEffector dialogueEffector;
        private static LocalizeStringEvent localeStringEvent;
        public static TextMeshProUGUI textRenderer;

        public static AudioSource textTypeSFX;
        public static AudioSource choiceSelectSFX;

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
            interactButton = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText/InteractButton")
                .GetComponent<Button>();
            interactButtonText = interactButton.GetComponentInChildren<TextMeshProUGUI>();
            textTypeSFX = GameObject.Find("/Managers/DialogueManager")
                .GetComponents<AudioSource>()[0];
            choiceSelectSFX = GameObject.Find("/Managers/DialogueManager")
                .GetComponents<AudioSource>()[1];
        }

        public void SetButtonListener(UnityAction listener)
        {
            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(listener);
        }
        
        public Button GetInteractButton()
        {
            return interactButton;
        }
        
        public void HideInteractionButton()
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.RemoveAllListeners();
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
            set => text = value;
        }

        public void ActivateDialogueFinishButton()
        {
            interactButtonText.text = "FINISH";
            interactButton.gameObject.SetActive(true);
        }
    }
}