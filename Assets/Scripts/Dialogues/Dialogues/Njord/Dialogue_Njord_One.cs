using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Dialogue_Njord_One : Dialogue
{


    public override IEnumerator ShowText(uint defaultDialogueIndex = 0)
    {
        while(currentDialogueIndex < GetDialogueCount())
        {
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

            DialogueManager.Instance.HideDefaultInteractionButton();
            
            if (currentDialogueData.Action == DIALOGUE_ACTION.FINISH ||
                currentDialogueIndex + 1 == GetDialogueCount())
            {
                ResterDialogues();
                OnDialogueFinished();
                StopAllCoroutines();
            }

            currentDialogueIndex++;
        }
        
        OnDialogueFinish.Invoke();

        currentDialogueIndex = 0;
        yield return null;
    }
    
    
        
    private void ResterDialogues()
    {
        choiceSelected = false;
        currentDialogueIndex = 0;
    }


    private void NoKronesOption()
    {
        choiceSelected = false;
        SetDialogue(4);
        currentDialogueIndex = 4;
    }
}