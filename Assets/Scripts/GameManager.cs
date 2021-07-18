using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    public bool isInputEnabled;
    public bool isInputEnabledUI;
    public bool IsPlayerAlive {get; set;} = false; // Siempre respawner al jugador on start!
    public int PlayerDeathCount = 0;
    public Camera CameraMain;

    private DynamicCamera dynCamera;

    internal Vector3 LastDeathPosition {get; set;}

    private GameManager()
    {
        CameraMain = Camera.main;
        if (CameraMain != null)
            dynCamera =
                CameraMain.GetComponent(typeof (DynamicCamera)) as
                DynamicCamera;

        isInputEnabled = false;
        isInputEnabledUI = true;
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
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

    public void SetInputEnabledUI(bool isEnabled)
    {
        if (instance != null) instance.isInputEnabledUI = isEnabled;
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
}
