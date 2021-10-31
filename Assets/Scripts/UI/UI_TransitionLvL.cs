using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UI_TransitionLvL : MonoBehaviour
    {
        [SerializeField] private bool showText = true;
        [SerializeField] private float duration = 8f;
        [SerializeField] private float delay;
        
        [SerializeField] private CanvasGroup mainCG;
        [SerializeField] private RectTransform contentToMove;
        
        [SerializeField] private GameObject loadingText;
        [SerializeField] private GameObject continueButton;

        private Vector3 initialPos;
        
        private void Start()
        {
            initialPos = contentToMove.localPosition;
        }

        public void StartTransition()
        {
            mainCG.interactable = true;
            StartCoroutine(RunTransition());
        }

        private IEnumerator RunTransition()
        {
            float counter = 0f;
            
            while (counter < duration)
            {
                counter += Time.deltaTime;
                mainCG.alpha = Mathf.Lerp(0, 1, counter / duration);

                yield return null;
            }
            GameManager.Instance.LevelWon();

            if (showText)
            {
                counter = 0f;
                duration = 20f;
                while (counter < duration)
                {
                    counter += Time.deltaTime;
                    contentToMove.localPosition += Vector3.up;
                    yield return null;
                }
            }

            yield return new WaitForSeconds(3f);

            loadingText.SetActive(false);
            continueButton.SetActive(true);
            
            if(GameManager.Instance.GetSceneIndex() == 0)
                GameManager.Instance.LoadLevel2();
            else if(GameManager.Instance.GetSceneIndex() == 1)
                GameManager.Instance.LoadLevel21();
            else if(GameManager.Instance.GetSceneIndex() == 2)
                GameManager.Instance.LoadLevel3();
        }

        private IEnumerator ResetTransition()
        {
            continueButton.SetActive(false);
            
            //GameManager.Instance.PlayerController.RespawnNow();
            
            float counter = 0f;

            duration = 3f;
            
            while (counter < duration)
            {
                counter += Time.deltaTime;
                mainCG.alpha = Mathf.Lerp(1, 0, counter / duration);

                yield return null;
            }
            
            loadingText.SetActive(true);
            contentToMove.localPosition = initialPos;
            gameObject.SetActive(false);

            GameManager.Instance.GetUIManager()
                .ShowLevelInfo();
        }

        public void RemoveTransition()
        {
            mainCG.interactable = false;
            StartCoroutine(ResetTransition());
        }
    }
}