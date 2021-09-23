using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Dialogues;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Dialogue_Njord_Zero : Dialogue
{

    private bool abilityGained;
    [SerializeField] private NjordController nController;

    public override IEnumerator ShowText(uint defaultDialogueIndex = 0)
    {
        while(currentDialogueIndex < GetDialogueCount())
        {
            if (abilityGained && currentDialogueIndex > 4)
            {
                OnDialogueFinish.Invoke();
                currentDialogueIndex = 99;
                yield break;
            }

            currentDialogueData = GetDialogueLocale((int)currentDialogueIndex);
            DialogueManager.Instance.GetLocalizeStrEvent().StringReference = currentDialogueData.Locale;
            
            yield return new WaitForSeconds(startDelay);
            
            for (int i = 0; i < DialogueManager.Text.Length + 1; i++)
            {
                currentText = DialogueManager.Text.Substring(0, i);
                DialogueManager.textRenderer.text = currentText;
                if (!String.IsNullOrEmpty(currentText) && !Char.IsWhiteSpace(currentText.Last()))
                    DialogueManager.textTypeSFX.Play();
                yield return new WaitForSeconds(delay);
            }

            if (currentDialogueData.Action == DIALOGUE_ACTION.NEXT &&
                !(currentDialogueIndex + 1 == GetDialogueCount()))
            {
                DialogueManager.Instance.SetDefaultInteractButtonText("NEXT");
            }
            
            if (currentDialogueData.Action == DIALOGUE_ACTION.CHOICE)
            {
                OnChoiceEvent(currentDialogueIndex);
            
                yield return new WaitUntil(() => choiceSelected);
            
                choiceSelected = false;
                
                if (!abilityGained)
                {
                    NoKronesOption();
                }
            
                choiceSelected = false;
            }

            DialogueManager.Instance.HideDefaultInteractionButton();
            
            if (currentDialogueData.Action == DIALOGUE_ACTION.FINISH ||
                currentDialogueIndex + 1 == GetDialogueCount())
            {
                ResterDialogues();
                OnDialogueFinished();
                StopAllCoroutines();
                DialogueManager.Instance.SetDefaultInteractButtonText("FINISH");
            }

            currentDialogueIndex++;
        }

        currentDialogueIndex = 0;
        yield return null;
    }
    
    
        
    private void ResterDialogues()
    {
        abilityGained = false;
        choiceSelected = false;
        currentDialogueIndex = 0;
    }


    private void NoKronesOption()
    {
        choiceSelected = false;
        SetDialogue(4);
        currentDialogueIndex = 4;
    }

    private protected override void OnChoiceEvent(uint dialogueIndex)
    {
        base.OnChoiceEvent(dialogueIndex);
        
        DialogueManager.Instance.SetButtonListener(() =>
        {
            if (dialogueIndex == 1 && GameManager.Instance.PlayerKrowns < 150)
            {
                abilityGained = false;
                //choiceSelectSFX.Play();
                ChooseOption(2);
                return;
            }
            OnDialogueChoseOption.Invoke(true);
            abilityGained = true;
            //choiceSelectSFX.Play();
            ChooseOption(2);
            
            GameManager.Instance.RemovePlayerKrowns(150);
            GameManager.Instance.PlayerController.powerUps.ChangePowerUp(GameManager.Instance.PlayerController.teleportPowerUp);
        });
    }
}