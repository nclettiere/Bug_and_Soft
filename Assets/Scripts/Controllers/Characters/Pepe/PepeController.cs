using System.Collections;
using System.Collections.Generic;
using Controllers.States;
using Dialogues;
using Interactions.Enums;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using World;

namespace Controllers
{
    public class PepeController : BaseController
    {
        [Header("Pepe : Dialogue Options")] private bool interacted;
        private DialogueGroup Dialogues { get; set; }

        [SerializeField] private QuickChatDialogue qcDialogue;
        [SerializeField] private Collider2D _collider;

        public bool isCompanion { get; private set; }

        public Pepe_IdleState IdleState { get; private set; }
        public Pepe_CompanionState CompanionState { get; private set; }

        private Vector3 initialPos;


        protected override void Start()
        {
            base.Start();

            Dialogues = GetComponent<DialogueGroup>();

            IdleState = new Pepe_IdleState(this, StateMachine, "Idle", this);
            CompanionState = new Pepe_CompanionState(this, StateMachine, "Following", this);

            StateMachine.Initialize(IdleState);
            
            initialPos = transform.position;

            GameManager.Instance.OnLevelReset.AddListener(ResetPepe);
        }

        private void ResetPepe()
        {
            if (GameManager.Instance.GetSceneIndex() == 0)
            {
                Dialogues.DialogueIndex = 0;
                StateMachine.ChangeState(IdleState);
            }

            transform.position = initialPos;

            interactionRadius = 4;
            canInteract = true;
            _collider.enabled = true;
            canPlayerInteract = true;
        }


        public override bool Interact(PlayerController controller, EInteractionKind interactionKind)
        {
            // disables the interaction bubble !!
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

        public void AcceptCompanion()
        {
            isCompanion = true;
            canInteract = false;
            canPlayerInteract = false;
            interactionRadius = 0;
            _collider.enabled = false;
            StateMachine.ChangeState(CompanionState);
        }

        public void ShowQuickChat(Sprite dialogueText)
        {
            qcDialogue.ShowNewChat(dialogueText);
        }

        public override void Flip()
        {
            base.Flip();
            qcDialogue.Flip();
        }
    }
}