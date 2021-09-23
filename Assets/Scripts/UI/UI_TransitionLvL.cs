using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_TransitionLvL : MonoBehaviour
    {
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
            
            counter = 0f;
            duration = 20f;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                contentToMove.localPosition += Vector3.up;
                yield return null;
            }
            
            yield return new WaitForSeconds(3f);

            // Mostramos el boton de continuar
            loadingText.SetActive(false);
            continueButton.SetActive(true);
        }

        private IEnumerator ResetTransition()
        {
            continueButton.SetActive(false);
            
            float counter = 0f;
            
            while (counter < duration)
            {
                counter += Time.deltaTime;
                mainCG.alpha = Mathf.Lerp(1, 0, counter / duration);

                yield return null;
            }
            
            loadingText.SetActive(true);
            contentToMove.localPosition = initialPos;
            gameObject.SetActive(false);
        }

        public void RemoveTransition()
        {
            mainCG.interactable = false;
            StartCoroutine(ResetTransition());
        }
    }
}