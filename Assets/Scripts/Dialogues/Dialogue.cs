using System.Collections;
using Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class Dialogue : MonoBehaviour
{
    
    public LocalizedString[] locales;
    public DIALOGUE_ACTION[] dialogueActions;
    
    private protected DialogueGroup dialogueGroup;
    private protected uint currentDialogueIndex;
    private protected DialogueData currentDialogueData;
    
    private protected string currentText = "";

    private protected float startDelay = 3f;
    private protected float delay = 0.1f;
    
    
    //private int from, to;
    private int current = -1;

    public void ShowDialogues()
    {
        StartCoroutine(ShowText());
    }
    
    public DialogueData GetDialogueLocale(int index)
    {
        if (index < locales.Length && index < dialogueActions.Length)
        {
            return new DialogueData(locales[index], dialogueActions[index], null);
        }

        return new DialogueData(null, DIALOGUE_ACTION.NEXT, null);
    }
    
    public int GetDialogueCount()
    {
        return locales.Length;
    }
    
    public DialogueData NextDialogue()
    {
        current++;
        return GetDialogueLocale(current);
    }

    public virtual IEnumerator ShowText()
    {
        yield return null;
    }

}