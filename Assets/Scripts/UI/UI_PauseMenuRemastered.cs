using System;
using Input;
using Managers;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UI_PauseMenuRemastered : MonoBehaviour, IJoystickInput
    {
        private GameObject ButtonPanels;
        private GameObject SaveLoadSlotsPanel;
        private GameObject SaveLoadSlotsOptionsPanel;
        [SerializeField] private GameObject[] SaveLoadSlotsOptionsButtons;
        [SerializeField] private GameObject ConfirmationPanel;
        
        private GameObject ReturnButton;
        private GameObject ReturnButtonSelector;

        private bool loadMenuOpened;
        
        public Button selectedButton;

        private float inputCooldown = float.NegativeInfinity;
        [SerializeField] private AudioSource onPlaySFX;

        private Vector2 UIControlPosition;
    
        public Button[] OrderedButtonsTransfroms;
        public Vector2[] OrderedButtonsLocalPositions;
        public GameObject ButtonSelector;

        private void Awake()
        {
            ButtonPanels = transform.Find("PauseMenuPanel/Buttons").gameObject;
            SaveLoadSlotsPanel = transform.Find("PauseMenuPanel/Content/SlotPanel").gameObject;
            SaveLoadSlotsOptionsPanel = transform.Find("PauseMenuPanel/Content/SlotOptionsPanel").gameObject;
            ReturnButton = transform.Find("PauseMenuPanel/ReturnButton").gameObject;
            ReturnButtonSelector = transform.Find("PauseMenuPanel/ReturnButtonSelector").gameObject;
        }

        private void Start()
        {
            OrderedButtonsTransfroms[0].onClick.AddListener(RemoveJoystickEvents);
        }

        private void OnEnable()
        {
            ConfirmationPanel.SetActive(false);
            ButtonPanels.SetActive(true);

            GameInput.playerControls.Gameplay.MenuInteract.performed += OnJoystickInteract;
            GameManager.Instance.gameInput.SetMenuJoystickInput(OnJoystickMovement);
        }

        private void OnDisable()
        {
            RemoveJoystickEvents();
        }

        public void OpenSaveLoadMenu()
        {
            loadMenuOpened = true;
            ButtonPanels.SetActive(false);
            SaveLoadSlotsPanel.SetActive(true);
            SaveLoadSlotsOptionsPanel.SetActive(true);
            ReturnButton.SetActive(true);
            ReturnButtonSelector.SetActive(true);
            
            foreach (var slotPanel in SaveLoadSlotsOptionsButtons)
            {
                slotPanel.GetComponent<CanvasGroup>().alpha = 1;
                slotPanel.GetComponent<Button>().interactable = true;
                slotPanel.GetComponent<UI_SlotButton>().UpdateSlotInfoData();
            }
        }
        
        public void CloseLoadMenu()
        {
            loadMenuOpened = false;
            ButtonPanels.SetActive(true);
            SaveLoadSlotsPanel.SetActive(false);
            SaveLoadSlotsOptionsPanel.SetActive(false);
            ReturnButton.SetActive(false);
            ReturnButtonSelector.SetActive(false);
        }

        public void SaveGameNow()
        {
            GameManager.Instance.QuickSave();
        }

        public void OpenLoadConfirmationPanel()
        {
            ButtonPanels.SetActive(false);
            ConfirmationPanel.SetActive(true);
        }
        
        public void CloseConfirmationPanel()
        {
            ButtonPanels.SetActive(true);
            ConfirmationPanel.SetActive(false);
        }
        
        public void LoadGameNow()
        {
            GameManager.Instance.QuickLoad();
        }
        
        public void GoBack()
        {
            if (loadMenuOpened)
            {
                CloseLoadMenu();
                return;
            }
        }
        
        public void BtnQuitCallback()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public void OnJoystickMovement(InputAction.CallbackContext context)
        {
            if (Time.time >= inputCooldown)
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