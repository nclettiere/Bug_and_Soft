using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UI_GameButton : MonoBehaviour
    {
        public EButtonFunction Function;
        public int saveSlot;
        public GameObject confirmationDialogue;
        public GameObject[] panelsToHide;
        public GameObject[] panelsToShow;
        
        public GameObject[] panelsAplhaZero;
        public GameObject[] panelsAlphaOne;
        
        public void OnClick()
        {
            switch (Function)
            {
                case EButtonFunction.QUIT:
                    GameManager.Instance.QuitGame();
                    break;
                case EButtonFunction.SETTINGS:
                    break;
                case EButtonFunction.MAINMENU:
                    RestartGame();
                    //GameManager.Instance = new GameManager();
                    break;
                case EButtonFunction.RETRY:
                    RestartGame();
                    //GameManager.Instance = new GameManager();
                    break;
                case EButtonFunction.RESUME:
                    GameManager.Instance.ResumeGame();
                    break;
                case EButtonFunction.SAVE:
                    SaveGameOnSlot();
                    break;
                case EButtonFunction.LOAD:
                    LoadGameOnSlot();
                    break;
                case EButtonFunction.SAVENOW:
                    SaveGameOnSlotNow();
                    break;
                case EButtonFunction.LOADNOW:
                    LoadGameOnSlotNow();
                    break;
            }
            
            foreach (var slotPanel in panelsToHide)
            {
                slotPanel.SetActive(false);
            }
            
            foreach (var panel in panelsToShow)
            {
                panel.SetActive(true);
            }
            
            foreach (var slotPanel in panelsAplhaZero)
            {
                slotPanel.GetComponent<CanvasGroup>().alpha = 0;
                slotPanel.GetComponent<Button>().interactable = false;
            }
            
            foreach (var panel in panelsAlphaOne)
            {
                panel.GetComponent<CanvasGroup>().alpha = 1;
                panel.GetComponent<Button>().interactable = true;
            }
        }

        private void DestroyAll()
        {
           
                GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);
 
                for (int i = 0; i < GameObjects.Length; i++)
                {
                    Destroy(GameObjects[i]);
                }
        }
        
        public void RestartGame()
        {
            //int scene = SceneManager.GetActiveScene().buildIndex;
            //SceneManager.UnloadScene(scene);
            //StartCoroutine(RestartLoad());

            GameManager.Instance.ReloadLevel();
        }

        private void SaveGameOnSlot()
        {
            GameManager.Instance.CreateNewSave(saveSlot);
        }
        
        private void LoadGameOnSlot()
        {
            GameManager.Instance.LoadSave(saveSlot);
        }
        
        private void SaveGameOnSlotNow()
        {
            confirmationDialogue.SetActive(false);
            
            GameManager.Instance.CreateNewSave(saveSlot);
        }
        
        private void LoadGameOnSlotNow()
        {
            GameManager.Instance.LoadSave(saveSlot);
        }

        private IEnumerator RestartLoad() 
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            yield return new WaitForEndOfFrame();
            // Reset the player here
            SceneManager.LoadScene(scene);
            
            GameManager.Instance.Retry();
        }
    }
}