using System;
using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using Controllers;
using Controllers.Froggy;
using Player;
using UI;
using UnityEditor;
using UnityEngine;

/// <summary>
///     <para>Esta clase controla el estado del juego.</para>
///     <para>Funciona como helper para todas las clases, ya que controla el UI, las camaras, la muerte del jugador, el guardado y la carga de savegames (no implementado), entre otros.</para>
///     <para>Se debe instanciar antes de usar <code>GameManager.Instance</code></para>
/// </summary>
/// <remarks>
///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
/// </remarks>
public class GameManager
{
    private static GameManager instance;
    private static bool sceneChanged;

    private bool isInputEnabled = true;
    private bool isMainMenuOn = true;
    private bool isGamePaused;
    private bool isPlayerAlive;
    private int playerDeathCount;

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
    
    public bool isGameOver {get; private set;}

    public static PlayerControls PlayerControls = new PlayerControls();


    public GameManager()
    {
        isInputEnabled = false;

        PlayerControls.Gameplay.Pause.performed += ctx =>
        {
            if (isMainMenuOn) return;

            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        };

        PauseGame();
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return instance;
        }
        set => throw new NotImplementedException();
    }

    public void PauseGame()
    {
        isGamePaused = true;
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
        if (!ReferenceEquals(GetDynamicCamera(), null)) instance.isInputEnabled = isEnabled;
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
        instance = new GameManager();
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
    }
}