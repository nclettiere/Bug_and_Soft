using Dialogues;
using UnityEngine;

namespace UI
{
    public class UI_ButtonDialogueSelector : MonoBehaviour
    {
        public void OnChoiceSelect()
        {
            Debug.Log("OnChoiceSelect");
            DialogueManager.Instance.ChooseOption(2);
        }
    }
}