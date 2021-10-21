using System.Collections;
using System.Collections.Generic;
using Dialogues;
using Interactions.Enums;
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

        protected override void Start()
        {
            base.Start();
            //Dialogues.DialogueIndex = 0;
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
            dialogueBubble.gameObject.SetActive(false);
            // Open the dialogue canvas
            DialogueManager.Instance.ShowDialogues(Dialogues);
            interacted = true;
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
    }
}