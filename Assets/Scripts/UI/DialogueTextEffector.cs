using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Dialogues;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class DialogueTextEffector : MonoBehaviour
{
    public float startDelay = 3f;
    public float delay = 0.1f;
    public string text;

    [SerializeField] private AudioSource textTypeSFX;
    [SerializeField] private Button dialogueNextButton;
    [SerializeField] private LocalizeStringEvent localeStringEvent;

    [SerializeField] private Button choiceButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button finishButton;

    private string currentText = "";
    private TextMeshProUGUI textRenderer;
    private DialogueGroup dialogues;
    private bool choiceSelected;
    private int selectedChoiceIndex;
    private DialogueData currentDialogueData;

    void Start()
    {
        textRenderer = GetComponent<TextMeshProUGUI>();
    }

    public void DisplayDialogues()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int ii = 0; ii < dialogues.GetDialogueCount(); ii++)
        {
            currentDialogueData = dialogues.NextDialogue();

            localeStringEvent.StringReference = currentDialogueData.Locale;

            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < Text.Length + 1; i++)
            {
                currentText = Text.Substring(0, i);
                textRenderer.text = currentText;
                if (!String.IsNullOrEmpty(currentText) && !Char.IsWhiteSpace(currentText.Last()))
                    textTypeSFX.Play();
                yield return new WaitForSeconds(delay);
            }

            if (currentDialogueData.Action == DIALOGUE_ACTION.CHOICE)
            {
                OnChoiceEvent();
                yield return new WaitUntil(() => choiceSelected);
                choiceSelected = false;
            }

            HideChoiceButtons();
        }

        OnEffectFinished();
    }

    private void OnChoiceEvent()
    {
        choiceButton.gameObject.SetActive(true);
        choiceButton.onClick.AddListener(() => currentDialogueData.RunAction());
    }

    private void OnEffectFinished()
    {
        finishButton.gameObject.SetActive(true);
        finishButton.onClick.AddListener(() => currentDialogueData.RunAction());
    }

    private void HideChoiceButtons()
    {
        choiceButton.gameObject.SetActive(false);
        choiceButton.onClick.RemoveAllListeners();
    }

    private void CloseDialogue()
    {
        DialogueManager.Instance.HideDialogues();
    }

    // Encapsulamos de esta manera, para ser usado por el Localization Table!
    public string Text
    {
        get => text;
        set => text = value;
    }

    public void SetDialogues(DialogueGroup dialogues)
    {
        StopAllCoroutines();
        this.dialogues = dialogues;
    }

    public void ChooseOption(int i)
    {
        choiceSelected = true;
        selectedChoiceIndex = i;
    }
}