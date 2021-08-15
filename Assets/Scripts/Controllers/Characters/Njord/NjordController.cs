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
        [Header("Njord : Dialogue Options")] [SerializeField]
        private DialogueGroup dialogues;

        private bool interacted;
        
        protected override void Start()
        {
            base.Start();

            controllerKind = EControllerKind.Neutral;
            InvokeRepeating("MoveCejas", 0f, 5f);
        }

        /// <summary>
        ///     Metodo para empezar la animacion de mover cejas del PJ Njord
        /// </summary>
        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }

        /// <summary>
        ///     Metodo para terminar la animacion de mover cejas del PJ Njord
        /// </summary>
        /// <remarks>
        ///     Es llamado al final de la animacion cejas.
        /// </remarks>
        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }

        public override bool Interact(PlayerController controller, EInteractionKind interactionKind)
        {
            if (!interacted)
            {
                // disables the interaction bubble !!
                dialogueBubble.gameObject.SetActive(false);
                // Open the dialogue canvas
                DialogueManager.Instance.ShowDialogues(dialogues);
                interacted = true;
                canInteract = false;
            }

            return true;
        }
    }
}