using System;
using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using Controllers;
using Controllers.Froggy;
using Player;
using UI;
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

    private bool isInputEnabled = true;
    private bool isMainMenuOn = true;
    private bool isGamePaused;
    private bool isPlayerAlive;
    private int playerDeathCount;
    private Camera cameraMain;
    private DynamicCamera dynCamera;
    private UI_HUD HUD;
    private Canvas HUDCanvas;
    public PlayerController PlayerController { get; }
    private PlayerControls playerControls;

    public int PlayerKrowns { get; private set; }

    public float DeltaTime
    {
        get { return isGamePaused ? 0 : Time.deltaTime; }
    }

    public Vector3 LastDeathPosition;
    private int mainMenuPhase;

    public bool IsFirstStart = true;

    private GameManager()
    {
        cameraMain = Camera.main;
        if (!ReferenceEquals(cameraMain, null))
            dynCamera = cameraMain.GetComponent<DynamicCamera>();

        playerControls = new PlayerControls();
        isInputEnabled = false;

        PlayerController = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
        HUD = GameObject.Find("UI/UI_HUD").GetComponent<UI_HUD>();
        HUDCanvas = GameObject.Find("UI/UI_HUD").GetComponent<Canvas>();

        playerControls.Gameplay.Pause.performed += ctx =>
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
            HUDCanvas.enabled = true;
            IsFirstStart = true;
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
        return playerControls;
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
        if (!ReferenceEquals(dynCamera, null) && isEnabled) dynCamera.UpdateSize(10f, 0.3f);
        if (!ReferenceEquals(dynCamera, null)) instance.isInputEnabled = isEnabled;
    }

    public void SetCameraTarget(Transform target)
    {
        if (dynCamera != null) dynCamera.ChangeTarget(target);
    }

    public void SetCameraOffset(Vector2 offset)
    {
        if (dynCamera != null) dynCamera.UpdateOffset(offset);
    }

    public void SetCameraOffsetX(float offsetX)
    {
        if (!ReferenceEquals(dynCamera, null)) dynCamera.UpdateOffsetX(offsetX);
    }

    public bool GetIsInputEnabled()
    {
        return isInputEnabled;
    }

    public void SetCameraOffsetY(float offsetY)
    {
        if (!ReferenceEquals(dynCamera, null)) dynCamera.UpdateOffsetY(offsetY);
    }

    public void SetCameraSize(float size, float duration = 3f)
    {
        if (dynCamera != null) dynCamera.UpdateSize(size, duration);
    }

    public Vector2 GetCameraOffset()
    {
        if (!ReferenceEquals(dynCamera, null))
            return dynCamera.GetOffsets();
        return Vector2.zero;
    }

    public void SetCameraFollowTarget(bool follow)
    {
        if (!ReferenceEquals(dynCamera, null))
            dynCamera.FollowTarget = follow;
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
        HUD.ShowBossHealth(bossName, controller);
    }

    public void HideBossHealth()
    {
        HUD.HideBossHealth();
    }
}