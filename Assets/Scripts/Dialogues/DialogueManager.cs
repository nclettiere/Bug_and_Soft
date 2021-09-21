using System;
using System.Collections.Generic;
using Dialogues.Dialogues;
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
        private static Button defaultInteractButton;
        private static TextMeshProUGUI defaultInteractButtonText;
        private static DialogueTextEffector dialogueEffector;
        private static LocalizeStringEvent localeStringEvent;
        public static TextMeshProUGUI textRenderer;
        public static GameObject customButtonsPanel;

        public static AudioSource textTypeSFX;
        public static AudioSource choiceSelectSFX;
        
        public static GameObject defaultButtonInteract;
        
        public static GameObject customButtonObject;
        
        private Stack<GameObject> customInteractionButtons;

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
            customButtonObject = Resources.Load("DefaultInteractButton") as GameObject;
            dialogueCanvas = GameObject.Find("/UI/UI_Dialogues").GetComponent<Canvas>();
            dialogueEffector = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<DialogueTextEffector>();
            localeStringEvent = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<LocalizeStringEvent>();
            textRenderer = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText")
                .GetComponent<TextMeshProUGUI>();
            defaultInteractButton = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText/InteractButtons/DefaultInteraction")
                .GetComponent<Button>();
            defaultInteractButtonText = defaultInteractButton.GetComponentInChildren<TextMeshProUGUI>();
            textTypeSFX = GameObject.Find("/Managers/DialogueManager")
                .GetComponents<AudioSource>()[0];
            choiceSelectSFX = GameObject.Find("/Managers/DialogueManager")
                .GetComponents<AudioSource>()[1];
            customButtonsPanel = GameObject.Find("/UI/UI_Dialogues/Content/DialogueText/InteractButtons");
        }

        public void SetButtonListener(UnityAction listener)
        {
            defaultInteractButton.onClick.RemoveAllListeners();
            defaultInteractButton.onClick.AddListener(listener);
        }
        
        public Button GetDefaultInteractButton()
        {
            return defaultInteractButton;
        }
        
        public void HideDefaultInteractionButton()
        {
            defaultInteractButton.gameObject.SetActive(false);
            defaultInteractButton.onClick.RemoveAllListeners();
        }

        public void AddInteractionButtons(InteractButton[] callbacks)
        {
            if (customInteractionButtons == null)
                customInteractionButtons = new Stack<GameObject>();

            ClearCustomButtons();
            
            foreach (var interactButton in callbacks)
            {
                if(interactButton.Callback == null)
                    continue;
                
                Debug.Log("Name: "+ customButtonObject);
                Debug.Log("Name: "+ customButtonObject.name);
                GameObject customButton = Instantiate(customButtonObject, Vector3.zero, Quaternion.Euler(0, -180, 0),
                    customButtonsPanel.transform);
                customButton.GetComponent<Button>()
                    .onClick.AddListener(interactButton.Callback);
                customButton.GetComponentInChildren<TextMeshProUGUI>()
                    .text = interactButton.Text;
                
                customButton.SetActive(true);
                
                customInteractionButtons.Push(customButton);
            }
        }

        public void ShowDialogues(DialogueGroup dialogues)
        {
            ClearCustomButtons();
            GameManager.Instance.EnterDialogueMode();
            dialogueCanvas.enabled = true;
            dialogues.Run();
        }

        public void HideDialogues()
        {
            dialogueCanvas.enabled = false;
            ClearCustomButtons();
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
            defaultInteractButtonText.text = "FINISH";
            defaultInteractButton.gameObject.SetActive(true);
        }
        
        public void SetDefaultInteractButtonText(string text)
        {
            defaultInteractButtonText.text = text;
        }

        public void ClearCustomButtons()
        {
            if (customInteractionButtons != null)
            {
                foreach (var button in customInteractionButtons)
                {
                    Destroy(button);
                }

                customInteractionButtons.Clear();
            }
        }
    }
}