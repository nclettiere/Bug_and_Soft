using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_InitialValues
{
    [Test]
    public void TestInitialValuesSimplePasses()
    {
        Assert.Null(GameManager.Instance);
        Assert.Null(GameManager.Instance.GetUIManager());
        Assert.Null(GameManager.Instance.GetHUD());
        Assert.IsTrue(GameManager.IsFirstStart);
        //Assert.AreEqual(100, PlayerController.maxHealth);
    }
    
    
}
