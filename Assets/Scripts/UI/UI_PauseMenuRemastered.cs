using SaveSystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_PauseMenuRemastered : MonoBehaviour
    {
        private GameObject ButtonPanels;
        private GameObject SaveLoadSlotsPanel;
        private GameObject SaveLoadSlotsOptionsPanel;
        [SerializeField] private GameObject[] SaveLoadSlotsOptionsButtons;
        
        private GameObject ReturnButton;
        private GameObject ReturnButtonSelector;

        private bool loadMenuOpened;

        private void Awake()
        {
            ButtonPanels = transform.Find("PauseMenuPanel/Buttons").gameObject;
            SaveLoadSlotsPanel = transform.Find("PauseMenuPanel/Content/SlotPanel").gameObject;
            SaveLoadSlotsOptionsPanel = transform.Find("PauseMenuPanel/Content/SlotOptionsPanel").gameObject;
            ReturnButton = transform.Find("PauseMenuPanel/ReturnButton").gameObject;
            ReturnButtonSelector = transform.Find("PauseMenuPanel/ReturnButtonSelector").gameObject;
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
        
        public void GoBack()
        {
            if (loadMenuOpened)
            {
                CloseLoadMenu();
                return;
            }
        }
    }
}