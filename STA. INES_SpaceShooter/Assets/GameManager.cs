using System.Collections;
using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector2 cameraBounds;

    public Vector2 CameraBounds
    {
        get { return cameraBounds; }
    }

    void Awake()
    {
        cameraBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
        instance = this;
        
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

   IEnumerator SpawnEnemies()
{
    while (true)
    {
        yield return new WaitForSeconds(1f);
        Vector2 spawnPosition = new Vector2(-cameraBounds.x, Random.Range(cameraBounds.y, -cameraBounds.y));

        
        ObjectsToPool enemyType = (ObjectsToPool)Random.Range((int)ObjectsToPool.enemyRed, (int)ObjectsToPool.enemyYellow + 1);

        ObjectPooling.instance.InstanceCreate(enemyType, spawnPosition);
    }
}
}