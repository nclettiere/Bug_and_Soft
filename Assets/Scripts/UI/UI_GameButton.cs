using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UI_GameButton : MonoBehaviour
    {
        public EButtonFunction Function;
        public int scene;

        public void OnClick()
        {
            scene = SceneManager.GetActiveScene().buildIndex;
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
            SceneManager.UnloadScene(scene);
            StartCoroutine(RestartLoad());
        }

        private IEnumerator RestartLoad() 
        {
            yield return new WaitForEndOfFrame();
            // Reset the player here
            SceneManager.LoadScene(scene);
            
            GameManager.Instance.Retry();
        }
    }
}