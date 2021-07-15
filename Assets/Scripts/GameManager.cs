using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    public bool isInputEnabled;
    public bool isInputEnabledUI;

    private GameManager()
    {
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
        if (instance != null)
            instance.isInputEnabled = isEnabled;
    }

    public void SetInputEnabledUI(bool isEnabled)
    {
        if (instance != null)
            instance.isInputEnabledUI = isEnabled;
    }
}
