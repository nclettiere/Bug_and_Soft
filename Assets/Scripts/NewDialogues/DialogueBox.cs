using System;
using Input;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace NewDialogues
{
    public class DialogueBox : MonoBehaviour
    {

        public UnityEvent OnSkip;
        public UnityEvent OnContinue;
        
        private void OnEnable()
        {
            GameInput.playerControls.Gameplay.MenuInteract.performed += OnJoystickInteract;
            GameInput.playerControls.Gameplay.DialoguesBack.performed += OnJoystickSkip;
            GameInput.playerControls.Gameplay.PauseL.performed += OnJoystickSkip;
        }

        private void OnDisable()
        {
            GameInput.playerControls.Gameplay.MenuInteract.performed -= OnJoystickInteract;
            GameInput.playerControls.Gameplay.DialoguesBack.performed -= OnJoystickSkip;
            GameInput.playerControls.Gameplay.PauseL.performed -= OnJoystickSkip;
        }

        public void OnJoystickInteract(InputAction.CallbackContext context)
        {
            OnContinue.Invoke(); 
        }

        public void OnJoystickSkip(InputAction.CallbackContext context)
        {
            OnSkip.Invoke(); 
        }
    }
}