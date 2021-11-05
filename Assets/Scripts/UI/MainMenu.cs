using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

/// <summary>
///     Script para el  menu principal 
/// </summary>
public class MainMenu : MonoBehaviour, IJoystickInput
{
    [SerializeField] private EventSystem _eventSystem;
    private GameObject pnlSettings, pnlButtons, pnlButtonsAnim, pnlButtonsInteraction, btnPlayAnim;
    private CanvasGroup dimmerCG;
    private Button btnPlay, btnSettings, btnApply, btnDiscard, btnQuit;
    private Dropdown dpwResolutions, dpwScreenMode, dpwLanguages;
    private Slider sldFPS, sldVolume;
    private Toggle togVSYNC;

    CanvasGroup panelButtonsGroup;
    CanvasGroup panelButtonsAnimGroup;
    CanvasGroup pnlButtonsInteractionGroup;
    CanvasGroup panelSettingsGroup;

    private List<Dropdown.OptionData> resolutionListDpw;

    private bool needToUpdateResolutions,
        needToUpdateScreenMode,
        needToUpdateVSYNC,
        needToUpdateFPS = false;

    public int sliderFPSamount, sliderVolumeAmount;

    public ScreenOpts[] screenOpts;

    public Button selectedButton;

    private float inputCooldown = float.NegativeInfinity;
    [SerializeField] private AudioSource onPlaySFX;

    private Vector2 UIControlPosition;
    
    public Button[] OrderedButtonsTransfroms;
    public Vector2[] OrderedButtonsLocalPositions;
    public GameObject ButtonSelector;
    public GameObject Logo;

    private void Start()
    {
        // Llamamos al GameManager para decirle que el MainMenu esta listo
        GameManager.Instance.SetMainMenuOn(true);

        // Registra los componentes del Canvas (UI) necesarios.
        pnlButtons = GameObject.Find("/UI/UI_MainMenu/PanelButtons");
        pnlButtonsAnim = GameObject.Find("/UI/UI_MainMenu/PanelButtonsAnims");
        pnlButtonsInteraction = GameObject.Find("/UI/UI_MainMenu/PanelRealButtons");
        pnlSettings = GameObject.Find("/UI/UI_MainMenu/PanelSettings");
        panelButtonsGroup = pnlButtons.GetComponent<CanvasGroup>();
        panelButtonsAnimGroup = pnlButtonsAnim.GetComponent<CanvasGroup>();
        panelSettingsGroup = pnlSettings.GetComponent<CanvasGroup>();
        pnlButtonsInteractionGroup = pnlButtonsInteraction.GetComponent<CanvasGroup>();

        dimmerCG = GameObject.Find("Dimmer").GetComponent<CanvasGroup>();
        btnPlay = GameObject.Find("ButtonPlay").GetComponent<Button>();
        btnPlayAnim = GameObject.Find("ButtonPlayAnim");
        btnSettings = GameObject.Find("ButtonSettings").GetComponent<Button>();
        btnApply = GameObject.Find("ButtonApply").GetComponent<Button>();
        btnDiscard = GameObject.Find("ButtonDiscard").GetComponent<Button>();
        btnQuit = GameObject.Find("ButtonQuit").GetComponent<Button>();
        dpwResolutions = GameObject.Find("DropdownResolutions").GetComponent<Dropdown>();
        dpwScreenMode = GameObject.Find("DropdownScreenMode").GetComponent<Dropdown>();
        dpwLanguages = GameObject.Find("DropdownLanguages").GetComponent<Dropdown>();
        sldFPS = GameObject.Find("SliderFPS").GetComponent<Slider>();
        sldVolume = GameObject.Find("SliderMasterVolume").GetComponent<Slider>();
        togVSYNC = GameObject.Find("ToggleVSYNC").GetComponent<Toggle>();

        // DEFAULT 
        btnPlayAnim.GetComponent<Image>().enabled = false;

        // Setea la lista del dpwResolutions con las resoluciones soportadas por
        // el monitor (vease el metodo Start).
        int selectedValue = PopulateResolutions();
        dpwResolutions.options = resolutionListDpw;
        dpwResolutions.value = selectedValue;

        // setea los datos por defecto de FPS
        SetLimitFPS();
        QualitySettings.vSyncCount = 1;
        sldFPS.interactable = false;
        sliderFPSamount = Screen.currentResolution.refreshRate;
        sldFPS.value = sliderFPSamount; // Para mejor performance setea los fps al refresh rate del monitor
        screenOpts[0].SetFPSStr(sliderFPSamount);
        screenOpts[1].SetVolumeStr(20);

        // Dropdown resolutions : event
        dpwResolutions.onValueChanged.AddListener(delegate { needToUpdateResolutions = true; });
        // Dropdown screenMode : event
        dpwScreenMode.onValueChanged.AddListener(delegate { needToUpdateScreenMode = true; });

        // Dropdown idiomas : event
        dpwLanguages.onValueChanged.AddListener(delegate { StartCoroutine(OnLocaleSelected(dpwLanguages.value)); });

        sldFPS.onValueChanged.AddListener(delegate
        {
            needToUpdateFPS = true;
            sliderFPSamount = (int) sldFPS.value;
            screenOpts[0].SetFPSStr(sliderFPSamount);
        });

        sldVolume.onValueChanged.AddListener(delegate
        {
            screenOpts[1].SetVolumeStr(sldVolume.value);
            GameManager.Instance.ChangeMasterVolume(sldVolume.value);
        });

        togVSYNC.onValueChanged.AddListener(delegate
        {
            sldFPS.interactable = !togVSYNC.isOn;
            needToUpdateVSYNC = true;
        });

        GameInput.playerControls.Gameplay.MenuInteract.performed += ctx =>
        {
            if (selectedButton != null && GameManager.Instance.GetMainMenuOn() &&
                GameManager.Instance.GetMainMenuPhase() == 0)
            {
                if (selectedButton != null)
                    selectedButton.onClick.Invoke();
            }
        };

        GameManager.Instance.gameInput.SetMenuJoystickInput(OnJoystickMovement);
    }

    /// <summary>
    /// Setea la lista resolutionListDpw que sera usada por el Dropdown de resoluciones
    /// </summary>
    /// <returns>Retorna el indice en el que se encuentra la resolucion que se esta usando en el momento.</returns>
    private int PopulateResolutions()
    {
        // Llena la lista de resoluciuones a usar por el Dropdown de res.
        resolutionListDpw = new List<Dropdown.OptionData>();
        Resolution[] resolutions = Screen.resolutions;
        Resolution current = Screen.currentResolution;

        int selectedIndex = 0;
        bool setted = false;
        foreach (var res in resolutions)
        {
            Dropdown.OptionData resData = new Dropdown.OptionData($"{res.width}x{res.height} : {res.refreshRate}");
            resolutionListDpw.Add(resData);

            if (!setted &&
                res.width != current.width ||
                res.height != current.height ||
                res.refreshRate != current.refreshRate)
                selectedIndex++;
            else
                setted = true;
        }

        return selectedIndex;
    }

    private FullScreenMode GetSelectedScreenMode()
    {
        int selected = dpwScreenMode.value;

        return selected == 0 ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    public void BtnPlayCallBack()
    {
        GameManager.Instance.SetMainMenuOn(false);
        onPlaySFX.Play();
        btnPlay.GetComponent<Image>().enabled = false;
        btnPlay.GetComponent<Button>().enabled = false;
        btnPlayAnim.GetComponent<Image>().enabled = true;
        btnSettings.GetComponent<Button>().enabled = false;
        btnQuit.GetComponent<Button>().enabled = false;

        GameManager.Instance.gameInput.RemoveJoystickInput();

        StopAllCoroutines();
        panelButtonsAnimGroup.alpha = 1f;
        StartCoroutine(DoFade(panelButtonsGroup, 1, 0, .3f));
        StartCoroutine(DoFade(panelButtonsAnimGroup, 1, 0, .5f, 1f, OnPlay));
    }

    private void OnPlay()
    {
        if (GameManager.Instance.GetSceneIndex() == 0)
        {
            Logo.SetActive(false);
            GameManager.Instance.LoadCutscene(1);
        }
        else
        {
            panelButtonsGroup.interactable = false;
            panelSettingsGroup.interactable = false;
            pnlButtonsInteractionGroup.interactable = false;
            GameManager.Instance.SetMainMenuOn(false);
            GameManager.Instance.SetInputEnabled(true);
            GameManager.Instance.SetCameraOffsetX(1.3f);
            GameManager.IsFirstStart = false;
            GameManager.Instance.ResumeGame();
            GameManager.Instance.ShowHUD();

            GameManager.Instance.GetUIManager()
                .ShowLevelInfo();
            
            Logo.SetActive(false);
        }
    }

    public void BtnSettingsCallBack()
    {
        GameManager.Instance.SetMainMenuPhase(1);
        
        _eventSystem.SetSelectedGameObject(dpwScreenMode.gameObject);

        panelButtonsGroup.interactable = false;
        panelSettingsGroup.interactable = true;
        pnlButtonsInteractionGroup.interactable = false;
        panelButtonsAnimGroup.interactable = false;

        StopAllCoroutines();

        StartCoroutine(DoFade(panelButtonsGroup, 1, 0, .3f));
        StartCoroutine(DoFade(panelButtonsAnimGroup, 1, 0, .3f));
        StartCoroutine(DoFade(panelSettingsGroup, 0, 1, .3f, 1f));
        StartCoroutine(DoFade(dimmerCG, 0, 1, .3f, 1f));

        panelButtonsAnimGroup.alpha = 0f;
    }

    public void BtnApplyCallBack()
    {
        Dropdown.OptionData resData = resolutionListDpw[dpwResolutions.value];
        string[] subsWidth = resData.text.Split('x');
        string[] subsHeight = subsWidth[1].Split(' ');

        int width = int.Parse(subsWidth[0]);
        int height = int.Parse(subsHeight[0]);
        int refreshRate = int.Parse(subsHeight[2]);

        if (needToUpdateScreenMode)
        {
            Screen.fullScreenMode = GetSelectedScreenMode();
        }

        if (needToUpdateResolutions)
        {
            Debug.Log(
                $"Applying resolution + refresh rate + screenmode ({width}x{height}:{refreshRate} - {GetSelectedScreenMode()})");
            Screen.SetResolution(width, height, GetSelectedScreenMode(), refreshRate);
        }

        if (needToUpdateVSYNC)
        {
            if (togVSYNC.isOn)
            {
                sldFPS.interactable = false;
                SetLimitFPS();
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                sldFPS.interactable = true;
                SetLimitFPS((int) sldFPS.value);
            }
        }

        if (needToUpdateFPS)
        {
            if (!togVSYNC.isOn)
                SetLimitFPS(sliderFPSamount);
        }

        CloseSettingsPanel();
    }

    public void CloseSettingsPanel()
    {
        GameManager.Instance.SetMainMenuPhase(0);
        panelButtonsGroup.interactable = true;
        pnlButtonsInteractionGroup.interactable = true;
        panelSettingsGroup.interactable = false;

        StopAllCoroutines();

        StartCoroutine(DoFade(panelButtonsGroup, 0, 1, .3f, 1f));
        panelButtonsAnimGroup.alpha = 1f;
        StartCoroutine(DoFade(dimmerCG, 1, 0, .3f, 1f));
        StartCoroutine(DoFade(panelSettingsGroup, 1, 0, .3f));
    }

    public void BtnQuitCallback()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Cambia el locale actual del juego.
    /// </summary>
    /// <param name="index">0 = en | 1 = es | 2 = it</param>
    /// <returns></returns>
    public IEnumerator OnLocaleSelected(int index)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    private void SetLimitFPS(int newFPS = 60)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = newFPS;
    }

    public IEnumerator DoFade(CanvasGroup cg, float start, float end, float duration, float delay = 0.0f,
        Action callback = null)
    {
        float counter = 0f;

        yield return new WaitForSeconds(delay);

        while (counter < duration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        if (callback != null)
            callback();
    }

    public void Show()
    {
        panelButtonsGroup.alpha = 1;
        dimmerCG.alpha = 0;
        panelButtonsAnimGroup.alpha = 1;
        panelSettingsGroup.alpha = 0;

        GameManager.Instance.gameInput.SetMenuJoystickInput(OnJoystickMovement);
    }

    public void Hide()
    {
        Logo.SetActive(false);
        panelButtonsAnimGroup.alpha = 0;
        OnPlay();
        panelButtonsAnimGroup.alpha = 0;

        GameManager.Instance.gameInput.RemoveJoystickInput();
    }

    public void OnJoystickMovement(InputAction.CallbackContext context)
    {
        if (Time.time >= inputCooldown && GameManager.Instance.isMainMenuOn)
        {
            Vector2 requestedPos = context.ReadValue<Vector2>();
            float newX = UIControlPosition.x + requestedPos.x;
            float newY = UIControlPosition.y + requestedPos.y;
            if (newX <= -1) newX = 0;
            if (newY <= -1) newY = 0;
        
            if (newX >= OrderedButtonsTransfroms.Length) newX = OrderedButtonsTransfroms.Length - 1;
            if (newY >= OrderedButtonsTransfroms.Length) newY = OrderedButtonsTransfroms.Length - 1;
        
            Vector2 tempNewSelectorPos = new Vector2(newX, newY);

            for (int i = 0; i < OrderedButtonsTransfroms.Length; i++)
            {
                if (OrderedButtonsLocalPositions[i] == tempNewSelectorPos)
                {
                    selectedButton = OrderedButtonsTransfroms[i];
                    ButtonSelector.transform.position = OrderedButtonsTransfroms[i].transform.position;
                    UIControlPosition = tempNewSelectorPos;
                    //SelectorChangeSFX.Play();
                    break;
                }
            }
        
            inputCooldown = Time.time + 0.1f;
        }
    }
}