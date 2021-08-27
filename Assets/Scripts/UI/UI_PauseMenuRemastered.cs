using UnityEngine;

namespace UI
{
    public class UI_PauseMenuRemastered : MonoBehaviour
    {
        private GameObject ButtonPanels;
        private GameObject SaveLoadSlotsPanel;
        private GameObject SaveLoadSlotsOptionsPanel;
        
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