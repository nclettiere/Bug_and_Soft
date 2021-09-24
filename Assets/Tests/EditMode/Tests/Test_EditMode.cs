using System.Collections;
using System.Collections.Generic;
using Managers;
using NUnit.Framework;
using Player;
using UnityEngine;
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
    public void PrimerCheck()
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
    /// Checkea que los valores iniciales sean los correctos
    /// </summary>
    [Test]
    public void SegundoCheck()
    {
        // GameManager.Instance deberia ser null
        // antes de que comienze el gioco
        Assert.IsNull(GameManager.Instance);
        // Check: Player empieza con 100 de vitta
        Assert.AreEqual(150, player.currentHealth);
        // El powerup port defecto tiene que ser NONE ;
        Assert.AreEqual(EPowerUpKind.NONE, uiManager.CurrentPowerUp);
    }
    
    [Test]
    public void TodoOkPapu()
    {
    }
}
