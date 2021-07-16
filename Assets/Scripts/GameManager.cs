using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    public bool isInputEnabled;

    public bool isInputEnabledUI;

    public Camera CameraMain;

    private DynamicCamera DynCamera;

    private GameManager()
    {
        CameraMain = Camera.main;
        if (CameraMain != null)
            DynCamera =
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

    public void SetInputEnabled(bool isEnabled)
    {
        if (DynCamera != null && isEnabled) DynCamera.UpdateSize(10f, 0.3f);

        if (instance != null) instance.isInputEnabled = isEnabled;
    }

    public void SetInputEnabledUI(bool isEnabled)
    {
        if (instance != null) instance.isInputEnabledUI = isEnabled;
    }

    public void SetCameraOffset(Vector2 offset)
    {
        if (DynCamera != null) DynCamera.UpdateOffset(offset);
    }

    public void SetCameraOffsetX(float offsetX)
    {
        if (DynCamera != null) DynCamera.UpdateOffsetX(offsetX);
    }

    public void SetCameraOffsetY(float offsetY)
    {
        if (DynCamera != null) DynCamera.UpdateOffsetY(offsetY);
    }

    public void SetCameraSize(float size)
    {
        if (DynCamera != null) DynCamera.UpdateSize(size);
    }
}
