using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using Managers;
using NUnit.Framework;
using Player;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

/// <summary>
/// Testeos del Playmode
/// +======================================================================+
/// Basicamente, checkeamos que esten los objetos cargados en la scene y,
/// simulamos input y chequeamos su respuesta.                           
/// +======================================================================+
/// </summary>
public class Test_PlayMode
{
    private bool sceneLoaded;
    private bool referencesSetup;
    private GameManager GM;
    private UIManager UIM;
    private PlayerController player;
    private DynamicCamera dyn;
 
       
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }
 
    void SetupReferences()
    {
        if (referencesSetup)
        {
            return;
        }
 
        Transform[] objects = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in objects)
        {
            switch (t.name)
            {
                case "GameManager":
                    GM = t.GetComponent<GameManager>();
                    break;
                case "UIManager":
                    UIM = t.GetComponent<UIManager>();
                    break;
                case "Player":
                    player = t.GetComponent<PlayerController>();
                    break;
                case "DynamicCamera":
                    dyn = t.GetComponent<DynamicCamera>();
                    break;
            }
        }
           
        referencesSetup = true;
    }
    
    [UnityTest]
    public IEnumerator TestObjectsInScene()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        SetupReferences();
        
        // Escencial
        Assert.IsNotNull(GM);
        Assert.IsNotNull(UIM);
        Assert.IsNotNull(player);
        Assert.IsNotNull(dyn);

        Debug.Log("Success! -> ObjectsInScene test passed");
        
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestGameManager()
    {
        Assert.IsNotNull(GameManager.Instance);
        Assert.AreEqual(1, GameManager.Instance.currentLevel);
        Assert.IsTrue(GameManager.Instance.isMainMenuOn);
        Debug.Log("Success! -> TestGameManager test passed");
        
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestUIManager()
    {
        Assert.IsNotNull(GameManager.Instance.GetUIManager());
        Assert.AreEqual(0, GameManager.Instance.GetUIManager().PowerUpState);
        Assert.AreEqual(EPowerUpKind.NONE, GameManager.Instance.GetUIManager().CurrentPowerUp);
        Assert.IsTrue(GameManager.Instance.isMainMenuOn);
        Debug.Log("Success! -> TestGameManager test passed");
        
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestPlayerInput()
    {
        player.SetHorizontalSpeed(1f);
        Assert.AreEqual(1f, player.hozSpeed);
        player.SetHorizontalSpeed(0f);
        Assert.AreEqual(0f, player.hozSpeed);
        Debug.Log("Success! -> PlayerInput test passed");
        
        yield return null;
    }
}
