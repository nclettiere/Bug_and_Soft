using System;
using UnityEngine;

namespace Dialogues
{
    public class DialogueManager
    {
        private static DialogueManager instance;
        private GameObject dialogueCanvas;
        
        private DialogueManager()
        {
            dialogueCanvas = GameObject.Find("/UI/UI_Dialogues");
        }

        public static DialogueManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DialogueManager();

                return instance;
            }
        }

        public void ShowDialogues()
        {
            dialogueCanvas.GetComponent<Canvas>().enabled = true;
            //dialogueCanvas.SetActive(true);
        }
        
        public void HideDialogues()
        {
            dialogueCanvas.GetComponent<Canvas>().enabled = false;
            //dialogueCanvas.SetActive(false);
        }
    }
}