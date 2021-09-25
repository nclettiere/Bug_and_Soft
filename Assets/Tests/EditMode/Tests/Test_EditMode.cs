using System.Collections;
using System.Collections.Generic;
using Managers;
using NUnit.Framework;
using Player;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.TestTools;

public class Test_EditMode
{
    private GameManager gm;
    private UIManager uiManager;
    private PlayerController player;
    
    /// <summary>
    /// Checkea que los objetos y clases esten dentro
    /// de los Gameobjects del nivel
    /// </summary>
    [Test]
    public void TestEssentialGameObjects()
    {
        gm = GameObject.Find("Managers/GameManager")
            .GetComponent<GameManager>();
        uiManager = GameObject.Find("Managers/UIManager")
            .GetComponent<UIManager>();
        player = GameObject.Find("Player")
            .GetComponent<PlayerController>();

        // El GameManager debe estar si o si en un GameObject
        Assert.IsNotNull(gm);
        // El UIManager debe estar si o si en un GameObject
        Assert.IsNotNull(uiManager);
        // Check: Player in scene
        Assert.IsNotNull(player);
    }

    /// <summary>
    /// Checkeamos que el LocalizationSettings este inicializado
    /// Es escencial para todos los textos del juego !!!!
    /// </summary>
    [Test]
    public void TestLocalization()
    {
        Assert.IsNotNull(LocalizationSettings.Instance);
    }
    
    [Test]
    public void TodoOkPapu()
    {
    }
}

