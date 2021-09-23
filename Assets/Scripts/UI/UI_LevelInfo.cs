using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_LevelInfo : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainCG;
        [SerializeField] private TextMeshProUGUI info;
        [SerializeField] private TextMeshProUGUI infoShadow;
        
        private AudioSource sound;

        private void Start()
        {
            sound = GetComponent<AudioSource>();
        }

        public void ShowInfo(int level, float delay)
        {
            string text = "";
            if (level == 1)
            {
                text = "Nivel 1:\nOLD VERGEN CHAMBER";
            }else if (level == 2)
            {
                text = "Nivel 2:\nDAMVERG RUINS";
            }
            
            info.text = text;
            infoShadow.text = text;

            StartCoroutine(Run(delay));
        }

        private IEnumerator Run(float delay)
        {
            float counter = 0f;
            float duration = 3f;
            
            sound.Play();
            
            while (counter < duration)
            {
                counter += Time.deltaTime;
                mainCG.alpha = Mathf.Lerp(0, 1, counter / duration);

                yield return null;
            }

            yield return new WaitForSeconds(2.5f);
            counter = 0f;
            duration = 1f;
            
            while (counter < duration)
            {
                counter += Time.deltaTime;
                mainCG.alpha = Mathf.Lerp(1, 0, counter / duration);

                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}