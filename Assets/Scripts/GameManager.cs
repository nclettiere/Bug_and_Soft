using System;
using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using Controllers;
using Controllers.Froggy;
using Player;
using SaveSystem.Data;
using UI;
using UnityEditor;
using UnityEngine;

public enum GameState { NullState, Intro, MainMenu, Game }
public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }
    
    public static GameManager Instance 
    { 
        get {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").AddComponent<GameManager>();
            }

            return _instance; 
        }
    }
    
    public void SetGameState(GameState gameState) {
        this.gameState = gameState;
        if(OnStateChange!=null) {
            OnStateChange();
        }
    }

    private static bool sceneChanged;

    private bool isInputEnabled = true;
    private bool isMainMenuOn = true;
    private bool isGamePaused;
    private bool isPlayerAlive;
    private int playerDeathCount;
    public int checkpointIndex { get; private set; }

    public int PlayerKrowns { get; private set; }

    public float DeltaTime
    {
        get { return isGamePaused ? 0 : Time.deltaTime; }
    }

    public PlayerController PlayerController
    {
        get
        {
            return GameObject.Find("Player")
                .gameObject.GetComponent<PlayerController>();
        }
    }

    public Vector3 LastDeathPosition;
    private int mainMenuPhase;

    public bool IsFirstStart = true;

    public bool isGameOver { get; private set; }

    public static PlayerControls PlayerControls = new PlayerControls();


    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject.GetComponent(_instance.GetType()));
        
        DontDestroyOnLoad(gameObject);          
        
        SetupGame();
        SetupInput();
    }

    private void SetupInput()
    {
        PlayerControls.Gameplay.Pause.performed += ctx =>
        {
            if (isMainMenuOn) return;
        
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        };
        
        PlayerControls.Gameplay.QuickSave.performed += ctx =>
        {
            if (isMainMenuOn || isGamePaused) return;
            QuickSave();
        };
        
        PlayerControls.Gameplay.QuickLoad.performed += ctx =>
        {
            if (isMainMenuOn || isGamePaused) return;
            QuickLoad();
        };
    }

    private void SetupGame()
    {
        isInputEnabled = false;
        PauseGame();
    }

    //public static GameManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //            instance = new GameManager();
//
    //        return instance;
    //    }
    //    set => throw new NotImplementedException();
    //}
    

    public void PauseGame()
    {
        Debug.Log("PAUSE GAME");
        GameObject.Find("/UI_PauseMenu2").SetActive(true);
        //GameObject.Find("UI/UI_GameOver").SetActive(false);
        //GameObject.Find("UI/UI_Dialogues").SetActive(false);
        //GameObject.Find("UI/UI_MainMenu").SetActive(false);
        //GameObject.Find("UI/UI_HUD").SetActive(false);
        //GameObject.Find("UI/UI_LevelCompleted").SetActive(false);
        //GameObject.Find("UI/UI_GameOver").SetActive(false);
        //isGamePaused = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;

        if (!IsFirstStart)
        {
            GetHUDCanvas().enabled = true;
        }
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    public bool GetMainMenuOn()
    {
        return isMainMenuOn;
    }

    public void SetMainMenuOn(bool isMainMenuOn)
    {
        this.isMainMenuOn = isMainMenuOn;
    }

    public PlayerControls GetPlayerControls()
    {
        return PlayerControls;
    }

    public Transform GetPlayerTransform()
    {
        return PlayerController.transform;
    }

    public void KillPlayer(Transform deathTransf)
    {
        LastDeathPosition = deathTransf.position;
        playerDeathCount++;
        SetInputEnabled(false);
        isPlayerAlive = false;
        SetCameraFollowTarget(false);
    }

    public void RespawnPlayer(bool isFirstRespawn = false)
    {
        if (!isFirstRespawn)
            SetInputEnabled(true);
        isPlayerAlive = true;
        SetCameraFollowTarget(true);
    }

    public void SetInputEnabled(bool isEnabled)
    {
        if (!ReferenceEquals(GetDynamicCamera(), null) && isEnabled) GetDynamicCamera().UpdateSize(10f, 0.3f);
        if (!ReferenceEquals(GetDynamicCamera(), null)) Instance.isInputEnabled = isEnabled;
    }

    public void SetCameraTarget(Transform target)
    {
        GetDynamicCamera().ChangeTarget(target);
    }

    public void SetCameraOffset(Vector2 offset)
    {
        GetDynamicCamera().UpdateOffset(offset);
    }

    public void SetCameraOffsetX(float offsetX)
    {
        GetDynamicCamera().UpdateOffsetX(offsetX);
    }

    public bool GetIsInputEnabled()
    {
        return isInputEnabled;
    }

    public void SetCameraOffsetY(float offsetY)
    {
        if (!ReferenceEquals(GetDynamicCamera(), null)) GetDynamicCamera().UpdateOffsetY(offsetY);
    }

    public void SetCameraSize(float size, float duration = 3f)
    {
        if (GetDynamicCamera() != null) GetDynamicCamera().UpdateSize(size, duration);
    }

    public Vector2 GetCameraOffset()
    {
        if (!ReferenceEquals(GetDynamicCamera(), null))
            return GetDynamicCamera().GetOffsets();
        return Vector2.zero;
    }

    public void SetCameraFollowTarget(bool follow)
    {
        if (!ReferenceEquals(GetDynamicCamera(), null))
            GetDynamicCamera().FollowTarget = follow;
    }

    public int GetMainMenuPhase()
    {
        return mainMenuPhase;
    }

    public void SetMainMenuPhase(int mainMenuPhase)
    {
        this.mainMenuPhase = mainMenuPhase;
    }

    public bool IsPlayerAlive()
    {
        return isPlayerAlive;
    }

    public int GetPlayerDeathCount()
    {
        return playerDeathCount;
    }

    public void PlayerExitInteractionMode()
    {
        PlayerController.ExitInteractionMode();
    }

    public bool AddPlayerKrowns(int amount)
    {
        PlayerController.KrownsAdded();
        PlayerKrowns += amount;
        return true;
    }

    public bool RemovePlayerKrowns(int amount)
    {
        if (PlayerKrowns >= amount)
        {
            PlayerController.KrownsRemoved();
            PlayerKrowns -= amount;
            return true;
        }

        return false;
    }

    public void ShowBossHealth(string bossName, BaseController controller)
    {
        GetHUD().ShowBossHealth(bossName, controller);
    }

    public void HideBossHealth()
    {
        GetHUD().HideBossHealth();
    }

    public void SetRomhoppState(int newState)
    {
        GetHUD().SetRomhoppState(newState);
    }

    public void ShowHUD()
    {
        GetHUDCanvas().enabled = true;
    }

    public void GameOver()
    {
        isGameOver = true;
        PauseGame();
        GameObject.Find("UI/UI_GameOver").GetComponent<Canvas>().enabled = true;
    }

    public void Retry()
    {
        //Instance = new GameManager();
        GameObject.Destroy(PlayerController.transform.gameObject);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public DynamicCamera GetDynamicCamera()
    {
        return Camera.main.GetComponent<DynamicCamera>();
    }

    public UI_HUD GetHUD()
    {
        return GameObject.Find("UI/UI_HUD").GetComponent<UI_HUD>();
    }

    public Canvas GetHUDCanvas()
    {
        return GameObject.Find("UI/UI_HUD").GetComponent<Canvas>();
    }

    public void LevelWon(int i)
    {
        isGameOver = true;
        PauseGame();
        GameObject.Find("UI/UI_LevelCompleted").GetComponent<Canvas>().enabled = true;
        GameObject.Find("UI/UI_GameOver").SetActive(false);
    }

    public bool CreateNewSave(int slot)
    {
        var playerData = new PlayerData(PlayerController);
        return SaveSystem.SaveSystem.SaveGame(ref playerData, slot);
    }

    public bool LoadSave(int slot)
    {
        var playerData = SaveSystem.SaveSystem.LoadSaveGame(slot);
        if (playerData == null) return false;

        // TODO: 1. loading screen 2. Reespawn enemies/bosses 3. Set player data
        PlayerController.SetData(playerData);

        return true;
    }

    public bool QuickSave()
    {
        var playerData = new PlayerData(PlayerController);
        return SaveSystem.SaveSystem.QuickSaveGame(ref playerData);
    }

    public bool QuickLoad()
    {
        var playerData = SaveSystem.SaveSystem.LoadQuickSaveGame();
        if (playerData == null) return false;

        // TODO: 1. loading screen 2. Reespawn enemies/bosses 3. Set player data
        PlayerController.SetData(playerData);

        return true;
    }

    public void SetPlayerKrones(int playerDataKrones)
    {
        PlayerKrowns = playerDataKrones;
    }
}