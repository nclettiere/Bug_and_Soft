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

namespace Controllers
{
    public class PepeController : BaseController
    {
        [Header("Pepe : Dialogue Options")] private bool interacted;
        public DialogueGroup Dialogues { get; private set; }

        [Header("Navigation")] private NavMeshAgent _agent;

        public bool isCompanion { get; private set; }

        public Pepe_IdleState IdleState { get; private set; }
        public Pepe_CompanionState CompanionState { get; private set; }

        protected override void Start()
        {
            base.Start();

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            Dialogues = GetComponent<DialogueGroup>();

            IdleState = new Pepe_IdleState(this, StateMachine, "Idle", this);
            CompanionState = new Pepe_CompanionState(this, StateMachine, "Following", this);

            StateMachine.Initialize(IdleState);

            controllerKind = EControllerKind.Neutral;
        }


        public override bool Interact(PlayerController controller, EInteractionKind interactionKind)
        {
            //if (!interacted)
            //{
            // disables the interaction bubble !!
            dialogueBubble.gameObject.SetActive(false);
            // Open the dialogue canvas
            DialogueManager.Instance.ShowDialogues(Dialogues);
            interacted = true;
            //canInteract = false;
            // }

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

        public void SetTargetDestination(Vector3 target)
        {
            _agent.SetDestination(target);
        }

        public void AcceptCompanion()
        {
            isCompanion = true;
            StateMachine.ChangeState(CompanionState);
        }
    }
}