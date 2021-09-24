using System.Collections;
using System.Collections.Generic;
using Managers;
using NUnit.Framework;
using Player;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_PlayMode
{
    private GameManager gm;
    private UIManager uiManager;
    private PlayerController player;

    [Test]
    public void PrimerTest()
    {
        gm = GameObject.Find("Managers/GameManager")
            .GetComponent<GameManager>();
        GameManager.Instance = gm;
        Assert.IsNotNull(GameManager.Instance);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Test_PlayModeWithEnumeratorPasses()
    {
        yield return null;
        // El GameManager debe estar si o si en un GameObject
        Assert.IsNotNull(GameManager.Instance);
        yield return null;
    }
}
