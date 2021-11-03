using System.Collections;
using System.Collections.Generic;
using Dialogues;
using Interactions.Enums;
using NewDialogues;
using Player;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Controllers
{
    public class NjordController : BaseController
    {
        [Header("Njord : Dialogue Options")] private bool interacted;
        [SerializeField] private DialogueGroup Dialogues;
        private NewDialogueGroup NewDialogues { get; set; }

        private bool _dialogueFinished;

        protected override void Start()
        {
            base.Start();
            
            NewDialogues = GetComponent<NewDialogueGroup>();
            InvokeRepeating("MoveCejas", 0f, 5f);
        }


        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }

        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }

        public override bool Interact(PlayerController controller, EInteractionKind interactionKind)
        {
            if (!_dialogueFinished && GameManager.Instance.GetSceneIndex() == 0)
            {
                dialogueBubble.gameObject.SetActive(false);
                FindObjectOfType<NewDialogueManager>().StartDialogue(NewDialogues);
                interacted = true;
            }
            else
            {
                OpenShop();
            }

            return true;
        }

        public void DialogueOnOptionChose(bool success)
        {
            if (success)
                Dialogues.DialogueIndex = 1;
        }

        public void DialogueOnFinish()
        {
            if (Dialogues.DialogueIndex == 1)
                Dialogues.DialogueIndex += 1;
        }

        public void OpenShop()
        {
            _dialogueFinished = true;
            GameManager.Instance
                .GetUIManager()
                .OpenShop();
        }
    }
}