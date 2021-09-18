using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Dialogue_Njord_Zero : Dialogue
{
    public override IEnumerator ShowText()
    {
        for (currentDialogueIndex = 0;
            currentDialogueIndex < GetDialogueCount() + 1;
            currentDialogueIndex++)
        {
            currentDialogueData = NextDialogue();
            
            DialogueManager.Instance.GetLocalizeStrEvent().StringReference = currentDialogueData.Locale;

            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < DialogueManager.Text.Length + 1; i++)
            {
                currentText = DialogueManager.Text.Substring(0, i);
                DialogueManager.textRenderer.text = currentText;
                //if (!String.IsNullOrEmpty(currentText) && !Char.IsWhiteSpace(currentText.Last()))
                //    textTypeSFX.Play();
                yield return new WaitForSeconds(delay);
            }

            //if (ii == 5)
            //    StopAllCoroutines();
//
            //if (currentDialogueData.Action == DIALOGUE_ACTION.CHOICE)
            //{
            //    OnChoiceEvent(ii);
//
            //    yield return new WaitUntil(() => choiceSelected);
//
            //    if (!abilityGained)
            //    {
            //        HideChoiceButtons();
            //        continue;
            //    }
//
            //    choiceSelected = false;
            //}
//
            //HideChoiceButtons();
//
            //if (currentDialogueData.Action == DIALOGUE_ACTION.FINISH)
            //{
            //    OnEffectFinished(ii);
            //    StopAllCoroutines();
            //}
        }
    }
}