using System;
using Controllers;
using Controllers.Froggy;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool hasEnemySpawned;
    private GameObject spawnedEnemy;
    private BaseController controller;

    [SerializeField] private GameObject character;
    [SerializeField] private ECharacterKind characterKind;

    private void Start()
    {
        GameManager.Instance.AddEnemySpawner(this);
    }

    public void Spawn()
    {
        if (!hasEnemySpawned)
        {
            spawnedEnemy = Instantiate(character, transform.position, Quaternion.Euler(0, 0, 0), transform);
            controller = spawnedEnemy.GetComponent<BaseController>();
            controller.OnLifeTimeEnded.AddListener(() => OnControllerDestroyed());
            hasEnemySpawned = true;
        }
    }

    private void OnControllerDestroyed()
    {
        hasEnemySpawned = false;
    }

    public BaseController GetController()
    {
        return controller;
    }

    public void Respawn()
    {
        if(spawnedEnemy != null)
            Destroy(spawnedEnemy);
        
        spawnedEnemy = Instantiate(character, transform.position, Quaternion.Euler(0, 0, 0), transform);
        controller = spawnedEnemy.GetComponent<BaseController>();
        controller.OnLifeTimeEnded.AddListener(() => OnControllerDestroyed());
        hasEnemySpawned = true;
    }
}