using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int maxEnemies = 3;
    [SerializeField] float spawnFrequency = 1f;

    public bool spawnTimer = true;
    static int numberOfEnemies = 0;

    public GameObject lastSpawn = null;
    public GameObject secondToLastSpawn = null;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void Update()
    {
        if (spawnTimer)
        {
            StartCoroutine(SpawnEnemies());
            spawnTimer = false;
        }
    }

    IEnumerator SpawnEnemies()
    {
        GameObject enemyToSpawn = null;
        enemyToSpawn = ChooseUniqueEnemey();

        yield return new WaitForSeconds(spawnFrequency);

        if (numberOfEnemies < maxEnemies)
        {
            
            KeepTrackOfSpawns(enemyToSpawn);
            Instantiate(enemyToSpawn, transform);
            numberOfEnemies++;
        }
        spawnTimer = true;
    }

    private GameObject ChooseUniqueEnemey()
    {
        GameObject enemyToSpawn;
        if (lastSpawn != null && secondToLastSpawn != null)
        {
            if (lastSpawn == secondToLastSpawn)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy != lastSpawn)
                    {
                        enemyToSpawn = enemy;
                        return enemyToSpawn;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                enemyToSpawn = enemies[Mathf.RoundToInt(UnityEngine.Random.Range(0, enemies.Length - 1))];
                return enemyToSpawn;
            }
        }
        else
        {
            enemyToSpawn = enemies[Mathf.RoundToInt(UnityEngine.Random.Range(0, enemies.Length - 1))];
            return enemyToSpawn;
        }
        return null;
    }

    private void KeepTrackOfSpawns(GameObject enemy)
    {
        secondToLastSpawn = lastSpawn;
        lastSpawn = enemy;
    }

    public void decreaseEnemyCount()
    {
        numberOfEnemies--;
    }
}
