using System.Collections;
using Enums;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject blindEffectIndicator;
        [SerializeField] private GameObject poisonEffectIndicator;
        [SerializeField] private GameObject UIShop;
        [SerializeField] private GameObject UILevelUpScreen;
        [SerializeField] private GameObject GameOverPanel;

        [SerializeField] private UI_TransitionLvL transitionLvl1;
        [SerializeField] private UI_TransitionLvL transitionLvl21;
        [SerializeField] private GameObject lvlCompletedPanel;
        [SerializeField] private UI_LevelInfo levelInfo;
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameObject mainMenuObject;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject DialoguePanel;

        public EPowerUpKind CurrentPowerUp { get; private set; }
        public uint PowerUpState { get; private set; }
        public bool IsShopOpened { get; private set; }
        public bool IsPauseMenuOpened { get; private set; }
        public bool IsDialoguePanelEnabled { get; set; }

        private bool isBlindnessActive;

        public void ApplyBlindness(float howLong)
        {
            if (!isBlindnessActive)
            {
                Volume volume = GetLevelPostProcess();
                Vignette vignette;
                volume.profile.TryGet(typeof(Vignette), out vignette);

                isBlindnessActive = true;

                StartCoroutine(BlindPlayerFor(vignette, howLong, 0f, 0.6f));
            }
        }

        private IEnumerator BlindPlayerFor(Vignette vignette, float seconds, float from, float to)
        {
            
            SetEffectActive(EEffectKind.BLINDNESS);
            
            float counter = 0f;
            while (counter < 0.4f)
            {
                counter += GameManager.Instance.DeltaTime;
                vignette.intensity.value = Mathf.Lerp(from, to, counter / 0.4f);

                yield return null;
            }

            yield return new WaitForSeconds(seconds);

            counter = 0f;
            
            DeactivateEffectIndicator(blindEffectIndicator);
            isBlindnessActive = false;
            
            while (counter < seconds)
            {
                counter += GameManager.Instance.DeltaTime;
                vignette.intensity.value = Mathf.Lerp(to, from, counter / 0.6f);

                yield return null;
            }
        }

        public void SetEffectActive(EEffectKind effectKind)
        {
            switch (effectKind)
            {
                case EEffectKind.BLINDNESS:
                    ActivateEffectIndicator(blindEffectIndicator, effectKind);
                    isBlindnessActive = true;
                    break;
            }
        }

        private void ActivateEffectIndicator(GameObject whatIndicator, EEffectKind effectKind)
        {
            whatIndicator.SetActive(true);
            //StartCoroutine(DeactivateEffectIndicator(whatIndicator, effectKind, howLong));
        }

        private IEnumerator DeactivateEffectIndicator(GameObject whatIndicator, EEffectKind effectKind, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            whatIndicator.SetActive(false);

            switch (effectKind)
            {
                case EEffectKind.BLINDNESS:
                    isBlindnessActive = false;
                    break;
            }
        }
        
        private void DeactivateEffectIndicator(GameObject whatIndicator)
        {
            whatIndicator.SetActive(false);
        }

        private Volume GetLevelPostProcess()
        {
            return GameObject.Find("PostProcess").GetComponent<Volume>();
        }
        
        public void ChangePowerUpKind(EPowerUpKind kind)
        {
            CurrentPowerUp = kind;
            GameManager.Instance.GetHUD().SwitchAbility();
        }

        public void ChangePowerUpState(uint state)
        {
            PowerUpState = state;
        }

        public void OpenShop()
        {
            IsShopOpened = true;
            GameManager.Instance.PauseGame();
            UIShop.SetActive(true);
        }

        public void CloseShop()
        {
            IsShopOpened = false;
            UIShop.SetActive(false);
            GameManager.Instance.ExitDialogueMode();
            GameManager.Instance.ResumeGame();
        }

        public void ShowTransitionOne()
        {
            HideLvlWonPanel();
            GameManager.Instance.HideBossHealth();
            transitionLvl1.gameObject.SetActive(true);
            transitionLvl1.StartTransition();
        }        
        
        public void ShowTransitionTwo()
        {
            HideLvlWonPanel();
            GameManager.Instance.HideBossHealth();
            transitionLvl21.gameObject.SetActive(true);
            transitionLvl21.StartTransition();
        }

        public void ShowLvlWonPanel()
        {
            lvlCompletedPanel.SetActive(true);
        }
        
        public void HideLvlWonPanel()
        {
            lvlCompletedPanel.SetActive(false);
        }

        public void ShowLevelInfo()
        {
            levelInfo.gameObject.SetActive(true);
            levelInfo.ShowInfo(GameManager.Instance.GetSceneIndex(), 4f);
        }        
        
        public void ShowLevelUpScreen()
        {
            UILevelUpScreen.SetActive(true);
        }
        
        public void HideLevelUpScreen()
        {
            UILevelUpScreen.SetActive(false);
        }

        public void LevelUpHealth()
        {
            HideLevelUpScreen();
            GameManager.Instance.AumentHealth();
            GameManager.Instance.GetPepeController().ShowLevelUpQC(0);
        }
        
        public void LevelUpDamage()
        {
            HideLevelUpScreen();
            GameManager.Instance.AumentDamage();
            GameManager.Instance.GetPepeController().ShowLevelUpQC(1);
        }
        
        public void LevelUpMagicka()
        {
            HideLevelUpScreen();
            GameManager.Instance.Aumentagicka();
            GameManager.Instance.GetPepeController().ShowLevelUpQC(2);
        }
        
        public void LevelUpCdr()
        {
            HideLevelUpScreen();
            GameManager.Instance.AumentCdr();
        }

        public void ShowGameOver()
        {
            GameOverPanel.SetActive(true);
        }     
        
        public void HideGameOver()
        {
            GameOverPanel.SetActive(false);
        }

        public void ShowMainMenu()
        {
            if(!mainMenuObject.activeSelf)
                mainMenuObject.SetActive(true);
            mainMenu.Show();
        }
        
        public void HideMainMenu()
        {
            mainMenuObject.SetActive(false);
        }

        public void OpenPauseMenu()
        {
            IsPauseMenuOpened = true;
            pauseMenu.SetActive(true);
        }
        
        public void ClosePauseMenu()
        {
            IsPauseMenuOpened = false;
            pauseMenu.SetActive(false);
            GameManager.Instance.ResumeGame();
        }

        public void EnableDialoguePanel()
        {
            IsDialoguePanelEnabled = true;
            DialoguePanel.SetActive(true);
        }
        
        public void DisableDialoguePanel()
        {
            IsDialoguePanelEnabled = false;
            DialoguePanel.SetActive(false);
        }

        public void UpdatePauseMenu()
        {
            pauseMenu.SetActive(GameManager.Instance.isGamePaused);
        }
    }
}