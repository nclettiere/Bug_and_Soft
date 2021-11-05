using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.States;
using Dialogues;
using Interactions.Enums;
using Inventory;
using JetBrains.Annotations;
using NewDialogues;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using World;
using DialogueManager = Dialogues.DialogueManager;

namespace Controllers
{
    public class PepeController : BaseController
    {
        [Header("Pepe : Dialogue Options")] private bool interacted;
        private DialogueGroup Dialogues { get; set; }
        private NewDialogueGroup NewDialogues { get; set; }

        [SerializeField] private QuickChatDialogue qcDialogue;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private bool _isLevel2Or3;
        
        [Header("Pepe : QuickChat sprites")]
        [SerializeField] private Sprite FirstHealQC;

        public Sprite[] OLCFlyQC;
        public Sprite[] Nievel2_1QC;
        public Sprite[] VergenDeadQC;
        public Sprite randomQC;
        public AudioSource Nievel2_1SFX;
        
        
        public Sprite[] LevelUpQC;
        public bool isCompanion { get; private set; }

        public Pepe_IdleState IdleState { get; private set; }
        public Pepe_CompanionState CompanionState { get; private set; }

        private Vector3 initialPos;
        
        private bool _levelOneInterected;

        protected override void Start()
        {
            base.Start();

            Dialogues = GetComponent<DialogueGroup>();
            NewDialogues = GetComponent<NewDialogueGroup>();

            IdleState = new Pepe_IdleState(this, StateMachine, "Idle", this);
            CompanionState = new Pepe_CompanionState(this, StateMachine, "Following", this);

            if (!_isLevel2Or3)
                StateMachine.Initialize(IdleState);
            else
                StateMachine.Initialize(CompanionState);

            initialPos = transform.position;

            if (GameManager.Instance.GetSceneIndex() == 2)
            {
                StartCoroutine(StartLevel21QuickChats());
            }
            
            GameManager.Instance.GetInventorySlotManager().OnFirstHeal.AddListener(()=>
            {
                ShowQuickChat(new Tuple<Sprite, int>(FirstHealQC, 3));
            });
            
            // Comentario random
            InvokeRepeating(nameof(ShowRandomQC), 40f, 40f);
        }

        private IEnumerator StartLevel21QuickChats()
        {
            yield return new WaitUntil(() => !GameManager.Instance.IsGamePaused());
            //Nievel2_1SFX.Play();
            yield return new WaitForSeconds(1);
            ShowQuickChat(new Tuple<Sprite, int>(Nievel2_1QC[0], 1));
            ShowQuickChat(new Tuple<Sprite, int>(Nievel2_1QC[1], 2));
            yield return 0;
        }

        protected override void Update()
        {
            base.Update();

            if (!_isLevel2Or3 && !_levelOneInterected)
            {
                var hit = Physics2D.CircleCast(transform.position, interactionRadius, Vector2.zero, 0,
                    ctrlData.whatIsPlayer);
                if (hit.collider != null && hit.collider.transform.CompareTag("Player"))
                {
                    _ = Interact(null);
                    _levelOneInterected = true;
                }
            }
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


        public override bool Interact([ItemCanBeNull] PlayerController controller, EInteractionKind interactionKind = EInteractionKind.Dialogue)
        {
            // disables the interaction bubble !!
            dialogueBubble.gameObject.SetActive(false);
            // Open the dialogue canvas
            //DialogueManager.Instance.ShowDialogues(Dialogues);
            FindObjectOfType<NewDialogueManager>().StartDialogue(NewDialogues);
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

        public void ShowQuickChat(Tuple<Sprite, int> quickChat)
        {
            qcDialogue.ShowNewChat(quickChat);
        }

        public override void Flip()
        {
            base.Flip();
            qcDialogue.Flip();
        }

        public void ShowVergenQuickChats()
        {
            ShowQuickChat(new Tuple<Sprite, int>(VergenDeadQC[0], 2));
            ShowQuickChat(new Tuple<Sprite, int>(VergenDeadQC[1], 2));
        }

        public void ShowRandomQC()
        {
            ShowQuickChat(new Tuple<Sprite, int>(randomQC, 1));
        }

        public void ShowLevelUpQC(int i)
        {
            ShowQuickChat(new Tuple<Sprite, int>(LevelUpQC[i], 1));
        }
    }
}