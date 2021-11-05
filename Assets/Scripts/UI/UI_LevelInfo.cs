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

        [SerializeField] private AudioSource sound;

        private void Start()
        {
            GameManager.Instance.OnLevelReset.AddListener(() => ShowInfo(GameManager.Instance.GetSceneIndex()));
        }

        public void ShowInfo(int level, float delay = 1f)
        {
            string text = "";
            if (level == 0)
            {
                text = "Nivel 1:\nOLD VERGEN CHAMBER";
            }
            if (level == 1)
            {
                text = "Nivel 2:\nSKADI-GROUND";
            }
            else if (level == 2)
            {
                text = "Nivel 3:\nDEMVERG RUINS";
            }
            else if (level == 3)
            {
                text = "Nivel 4:\nVERGEN CHAMBER";
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