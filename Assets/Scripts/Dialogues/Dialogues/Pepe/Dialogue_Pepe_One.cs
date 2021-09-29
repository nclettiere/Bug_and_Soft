using System;
using System.Collections;
using System.Linq;
using Controllers;
using UnityEngine;

namespace Dialogues.Dialogues.Pepe
{
    public class Dialogue_Pepe_One : Dialogue
    {
        
        [SerializeField] private PepeController pController;

        public override IEnumerator ShowText(uint defaultDialogueIndex = 0)
        {
            while (currentDialogueIndex < GetDialogueCount())
            {
                currentDialogueData = GetDialogueLocale((int) currentDialogueIndex);
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
                
                InteractButton[] interactButtons = new InteractButton[1];
                interactButtons[0] = new InteractButton();
                interactButtons[0].Text = "SALTAR";
                interactButtons[0].Callback = () =>
                {
                    ResterDialogues();
                    OnDialogueFinished();
                    pController.AcceptCompanion();
                    currentDialogueIndex = 99;
                };
                DialogueManager.Instance.AddInteractionButtons(interactButtons);

                if (currentDialogueData.Action == DIALOGUE_ACTION.FINISH ||
                    currentDialogueIndex + 1 == GetDialogueCount())
                {
                    ResterDialogues();
                    OnDialogueFinished();
                    pController.AcceptCompanion();
                    StopAllCoroutines();
                }

                currentDialogueIndex++;
            }

            OnDialogueFinish.Invoke();

            ResterDialogues();
            DialogueManager.Instance.ClearCustomButtons();

            currentDialogueIndex = 0;
            yield return null;
        }

        private void ResterDialogues()
        {
            choiceSelected = false;
            currentDialogueIndex = 0;
            //DialogueManager.Instance.ClearCustomButtons();
        }
    }
}