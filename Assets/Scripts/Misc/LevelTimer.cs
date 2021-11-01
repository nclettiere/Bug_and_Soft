using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public class LevelTimer : MonoBehaviour
    {
        public float timerWaitAmout = 120;
        public float delay = 10;
        public bool timerIsRunning = false;

        public UnityEvent OnTimerEnd;
        public TextMeshProUGUI[] TextTimers;

        private string lastFormattedTime;
        
        private float timeRemaining;
        
        private void Awake()
        {
            if (OnTimerEnd == null)
                OnTimerEnd = new UnityEvent();
        }

        private void Start()
        {
            timeRemaining = timerWaitAmout;
            if (delay > 0)
                StartCoroutine(StartWithDelay());
            else
                timerIsRunning = true;
            GameManager.Instance.OnLevelReset.AddListener(() =>
            {
                timeRemaining = timerWaitAmout;
                timerIsRunning = true;
            });
            OnTimerEnd.AddListener(GameManager.Instance.GameOver);
        }

        private IEnumerator StartWithDelay()
        {
            yield return new WaitForSeconds(delay);
            timerIsRunning = true;
            yield return 0;
        }

        void Update()
        {
            if (timerIsRunning && !GameManager.Instance.IsGamePaused())
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    lastFormattedTime = DisplayTime();
                }
                else
                {
                    OnTimerEnd.Invoke();
                    timeRemaining = 0;
                    timerIsRunning = false;
                }

                foreach (var Text in TextTimers)
                {
                    Text.text = lastFormattedTime;
                }
            }
        }
        
        public string DisplayTime()
        {
            float minutes = Mathf.FloorToInt(timeRemaining / 60); 
            float seconds = Mathf.FloorToInt(timeRemaining % 60);

            if (minutes < 0)
                minutes = 0;
            if (seconds < 0)
                seconds = 0;
            
            return $"{minutes:00}:{seconds:00}";
        }

        public void Cancel()
        {
            timerIsRunning = false;
            timeRemaining = timerWaitAmout;
        }
        
        public void Pause()
        {
            timerIsRunning = false;
        }
        
        public void Resume()
        {
            timerIsRunning = true;
        }
    }
}