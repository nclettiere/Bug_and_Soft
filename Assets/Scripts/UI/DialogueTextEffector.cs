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
    [SerializeField] private AudioSource choiceSelectSFX;
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
    private bool abilityGained;

    void Start()
    {
        textRenderer = GetComponent<TextMeshProUGUI>();
    }

    public void DisplayDialogues()
    {
        StartCoroutine(ShowText());
    }
    
    int ii;

    IEnumerator ShowText()
    {
        //for (ii = 0; ii < dialogues.GetDialogueCount() + 1; ii++)
        //{
        //    if (ii > 1 && !abilityGained)
        //    {
//
        //        currentDialogueData = dialogues.GetDialogueLocale(5);
        //        ii = 5;
        //    }
        //    else
        //    {
        //        currentDialogueData = dialogues.NextDialogue();
        //    }
//
        //    localeStringEvent.StringReference = currentDialogueData.Locale;
//
        //    yield return new WaitForSeconds(startDelay);
//
        //    for (int i = 0; i < Text.Length + 1; i++)
        //    {
        //        currentText = Text.Substring(0, i);
        //        textRenderer.text = currentText;
        //        if (!String.IsNullOrEmpty(currentText) && !Char.IsWhiteSpace(currentText.Last()))
        //            textTypeSFX.Play();
        //        yield return new WaitForSeconds(delay);
        //    }
//
        //    if (ii == 5)
        //        StopAllCoroutines();
//
        //    if (currentDialogueData.Action == DIALOGUE_ACTION.CHOICE)
        //    {
        //        OnChoiceEvent(ii);
        //        
        //        yield return new WaitUntil(() => choiceSelected);
        //        
        //        if (!abilityGained)
        //        {
        //            HideChoiceButtons();
        //            continue;
        //        }
//
        //        choiceSelected = false;
        //    }
//
        //    HideChoiceButtons();
        //    
        //    if (currentDialogueData.Action == DIALOGUE_ACTION.FINISH)
        //    {
        //        OnEffectFinished(ii);
        //        StopAllCoroutines();
        //    }
        //}
        yield return null;
    }
    

    private void OnChoiceEvent(int dialogueIndex)
    {
        choiceButton.gameObject.SetActive(true);
        
        // TODO: Para el segundo sprint implementar RunAction con UnityEvent
        //choiceButton.onClick.AddListener(() => currentDialogueData.RunAction());
        
        // Temporal, hasta el segundo sprint !
        choiceButton.onClick.AddListener(() =>
        {
            if (dialogueIndex == 1 && GameManager.Instance.PlayerKrowns < 150)
            {
                Debug.Log("krones are not sufficient");
                abilityGained = false;
                choiceSelectSFX.Play();
                ChooseOption(2);
                return;
            }
            
            abilityGained = true;
            choiceSelectSFX.Play();
            ChooseOption(2);
            
            GameManager.Instance.RemovePlayerKrowns(150);
            GameManager.Instance.PlayerController.powerUps.ChangePowerUp(GameManager.Instance.PlayerController.teleportPowerUp);
        });
    }

    private void OnEffectFinished(int index)
    {
        finishButton.gameObject.SetActive(true);
        finishButton.onClick.AddListener(() =>
        {
            DialogueManager.Instance.HideDialogues();
            GameManager.Instance.PlayerExitInteractionMode();
            GameManager.Instance.ResumeGame();
        });
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