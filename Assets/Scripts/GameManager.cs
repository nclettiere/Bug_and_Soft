using System;
using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using UnityEngine;

/// <summary>
///     <para>Esta clase controla el estado del juego.</para>
///     <para>Funciona como helper para todas las clases, ya que controla el UI, las camaras, la muerte del jugador, el guardado y la carga de savegames, entre otros.</para>
///     <para>Se debe instanciar antes de usar <code>GameManager.Instance</code></para>
/// </summary>
/// <remarks>
///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
/// </remarks>
public class GameManager
{
    private static GameManager instance;

    private bool isInputEnabled;
    private bool isMainMenuOn = true;
    public bool IsPlayerAlive {get; set;} = false; // Siempre respawner al jugador on start!
    public int PlayerDeathCount = 0;
    public Camera CameraMain;

    private DynamicCamera dynCamera;

    public PlayerControls playerControls;

    internal Vector3 LastDeathPosition {get; set;}

    private int mainMenuPhase = 0;

    private GameManager()
    {
        CameraMain = Camera.main;
        if (CameraMain != null)
            dynCamera =
                CameraMain.GetComponent(typeof (DynamicCamera)) as
                DynamicCamera;

        playerControls = new PlayerControls();
        isInputEnabled = false;
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

    public bool GetMainMenuOn()
    {
        return isMainMenuOn;
    }

    public void SetMainMenuOn(bool isMainMenuOn)
    {
        this.isMainMenuOn = isMainMenuOn;
    }

    public void KillPlayer(Transform deathTransf)
    {
        LastDeathPosition = deathTransf.position;
        PlayerDeathCount++;
        SetInputEnabled(false);
        IsPlayerAlive = false;
        SetCameraFollowTarget(false);
    }

    public void RespawnPlayer(bool isFirstRespawn = false)
    {
        if(!isFirstRespawn)
            SetInputEnabled(true);
        IsPlayerAlive = true;
        SetCameraFollowTarget(true);
    }

    public void SetInputEnabled(bool isEnabled)
    {
        if (dynCamera != null && isEnabled) dynCamera.UpdateSize(10f, 0.3f);

        if (instance != null) instance.isInputEnabled = isEnabled;
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
        if (dynCamera != null) dynCamera.UpdateOffsetX(offsetX);
    }

    public bool GetIsInputEnabled()
    {
        return isInputEnabled;
    }

    public void SetCameraOffsetY(float offsetY)
    {
        if (dynCamera != null) dynCamera.UpdateOffsetY(offsetY);
    }

    public void SetCameraSize(float size)
    {
        if (dynCamera != null) dynCamera.UpdateSize(size);
    }

    public Vector2 GetCameraOffset()
    {
        if (dynCamera != null)
            return dynCamera.GetOffsets();
        return Vector2.zero;
    }
    public void SetCameraFollowTarget(bool follow)
    {
        if (dynCamera != null)
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
}
