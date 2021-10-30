using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    
    void Update()
    {
        if(GameManager.Instance != null)
            CheckGamePaused();
    }

    private void CheckGamePaused() 
    {
        if(
            !GameManager.Instance.GetMainMenuOn() && 
            !GameManager.IsFirstStart && 
            !GameManager.Instance.isGameOver && 
            !GameManager.Instance.isDialogueMode &&
            !GameManager.Instance.isLevelingUp &&
            !GameManager.Instance.GetUIManager().IsShopOpened)
            pauseMenu.gameObject.SetActive(GameManager.Instance.IsGamePaused());
    }
}
