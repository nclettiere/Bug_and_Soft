using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UI_GameButton : MonoBehaviour
    {
        public EButtonFunction Function;
        public int saveSlot;
        public GameObject confirmationDialogue;
        public GameObject[] panelsToHide;
        public GameObject[] panelsToShow;
        
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
                    GameManager.Instance = new GameManager();
                    break;
                case EButtonFunction.RETRY:
                    RestartGame();
                    GameManager.Instance = new GameManager();
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
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.UnloadScene(scene);
            StartCoroutine(RestartLoad());
        }

        private void SaveGameOnSlot()
        {
            confirmationDialogue.SetActive(true);

            foreach (var slotPanel in panelsToHide)
            {
                slotPanel.SetActive(false);
            }
            
            GameManager.Instance.CreateNewSave(saveSlot);
        }
        
        private void LoadGameOnSlot()
        {
            GameManager.Instance.LoadSave(saveSlot);
        }
        
        private void SaveGameOnSlotNow()
        {
            confirmationDialogue.SetActive(false);
            
            foreach (var slotPanel in panelsToHide)
            {
                slotPanel.SetActive(false);
            }
            
            foreach (var panel in panelsToShow)
            {
                panel.SetActive(true);
            }
            
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