using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    
    void Update()
    {
        CheckGamePaused();
    }

    private void CheckGamePaused() 
    {
        if(!GameManager.Instance.GetMainMenuOn() && !GameManager.IsFirstStart && !GameManager.Instance.isGameOver && !GameManager.Instance.isDialogueMode)
            pauseMenu.gameObject.SetActive(GameManager.Instance.IsGamePaused());
    }
}
