using System;
using Input;
using Managers;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UI_GameOver : MonoBehaviour, IJoystickInput
    {
                
        public Button selectedButton;

        private float inputCooldown = float.NegativeInfinity;
        [SerializeField] private AudioSource onPlaySFX;

        private Vector2 UIControlPosition;
    
        public Button[] OrderedButtonsTransfroms;
        public Vector2[] OrderedButtonsLocalPositions;
        public GameObject ButtonSelector;

        private bool isEnabled;
        
        private void Start()
        {
            OrderedButtonsTransfroms[0].onClick.AddListener(RemoveJoystickEvents);
        }

        private void OnEnable()
        {
            isEnabled = true;
            GameInput.playerControls.Gameplay.MenuInteract.performed += OnJoystickInteract;
            GameManager.Instance.gameInput.SetMenuJoystickInput(OnJoystickMovement);
        }

        private void OnDisable()
        {
            isEnabled = true;
            RemoveJoystickEvents();
        }

        public void OnJoystickMovement(InputAction.CallbackContext context)
        {            
            if (Time.time >= inputCooldown && isEnabled && enabled && transform.gameObject.activeSelf)
            {
                Vector2 requestedPos = context.ReadValue<Vector2>();
                float newX = UIControlPosition.x + requestedPos.x;
                float newY = UIControlPosition.y + requestedPos.y;
                if (newX <= -1) newX = 0;
                if (newY <= -1) newY = 0;
        
                if (newX >= OrderedButtonsTransfroms.Length) newX = OrderedButtonsTransfroms.Length - 1;
                if (newY >= OrderedButtonsTransfroms.Length) newY = OrderedButtonsTransfroms.Length - 1;
        
                Vector2 tempNewSelectorPos = new Vector2(newX, newY);

                for (int i = 0; i < OrderedButtonsTransfroms.Length; i++)
                {
                    if (OrderedButtonsLocalPositions[i] == tempNewSelectorPos)
                    {
                        selectedButton = OrderedButtonsTransfroms[i];
                        ButtonSelector.transform.position = OrderedButtonsTransfroms[i].transform.position;
                        UIControlPosition = tempNewSelectorPos;
                        //SelectorChangeSFX.Play();
                        break;
                    }
                }
        
                inputCooldown = Time.time + 0.1f;
            }
        }
        
        public void OnJoystickInteract(InputAction.CallbackContext context)
        {
            if (selectedButton != null)
                selectedButton.onClick.Invoke();
        }
        
        public void RemoveJoystickEvents()
        {
            GameManager.Instance.gameInput.RemoveJoystickInput();
            GameInput.playerControls.Gameplay.MenuInteract.performed -= OnJoystickInteract;
        }
    }
}