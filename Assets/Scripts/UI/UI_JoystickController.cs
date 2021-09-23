using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_JoystickController : MonoBehaviour
{
    public GameObject ButtonSelector;
    public Button[] OrderedButtonsTransfroms;
    public Vector2[] OrderedButtonsLocalPositions;
    public AudioSource SelectorChangeSFX;
    private float inputCooldown = float.NegativeInfinity;
    private Vector2 UIControlPosition;
    public Button SelectedButton;

    public bool IsMainMenu;
    public int MainMenuPhase = 0;

    private void Start()
    {
        //GameManager.Instance.GetPlayerControls().Gameplay.MenuMovement.performed += ctx =>
        //{
        //    if (IsMainMenu)
        //    {
        //        if (!GameManager.Instance.GetMainMenuOn() || GameManager.Instance.GetMainMenuPhase() != MainMenuPhase)
        //            return;
        //    }
        //    else
        //    {
        //        // Pause
        //        if (!GameManager.Instance.IsGamePaused())
        //            return; 
        //    }
//
        //    if (Time.time >= inputCooldown)
        //    {
        //        Vector2 requestedPos = ctx.ReadValue<Vector2>();
        //        float newX = UIControlPosition.x + requestedPos.x;
        //        float newY = UIControlPosition.y + requestedPos.y;
        //        if (newX <= -1) newX = 0;
        //        if (newY <= -1) newY = 0;
//
        //        if (newX >= OrderedButtonsTransfroms.Length) newX = OrderedButtonsTransfroms.Length - 1;
        //        if (newY >= OrderedButtonsTransfroms.Length) newY = OrderedButtonsTransfroms.Length - 1;
//
        //        Vector2 tempNewSelectorPos = new Vector2(newX, newY);
//
        //        for (int i = 0; i < OrderedButtonsTransfroms.Length; i++)
        //        {
        //            if (OrderedButtonsLocalPositions[i] == tempNewSelectorPos)
        //            {
        //                SelectedButton = OrderedButtonsTransfroms[i];
        //                ButtonSelector.transform.position = OrderedButtonsTransfroms[i].transform.position;
        //                UIControlPosition = tempNewSelectorPos;
        //                SelectorChangeSFX.Play();
        //                break;
        //            }
        //        }
//
        //        inputCooldown = Time.time + 0.1f;
        //    }
        //};
        //
        //GameManager.Instance.GetPlayerControls().Gameplay.MenuInteract.performed += ctx =>
        //{
        //    if (SelectedButton != null && GameManager.Instance.GetMainMenuOn() && GameManager.Instance.GetMainMenuPhase() == 0)
        //    {
        //        if (SelectedButton != null)
        //            SelectedButton.onClick.Invoke();
        //    }
        //};

    }
}
