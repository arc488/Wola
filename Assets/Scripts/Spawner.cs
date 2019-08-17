using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float spawnFrequency = 1f;
    [SerializeField] Progression progression = null;
    [SerializeField] float enemyNumberMultiplier = 10f;
    [SerializeField] float countdownLength = 5f;

    public bool spawnTimer = true;

    public SpawnerManager spawnerManager;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void Start()
    {
        spawnerManager = SpawnerManager.Instance;
        spawnerManager.maxEnemies = progression.GetSpawnLevel(spawnerManager.currentLevel) * enemyNumberMultiplier;
    }

    private void Update()
    {

        if (spawnerManager.isCountdownActive) spawnerManager.levelCountdown += Time.deltaTime;

        CheckIfLevelCompleted();


        if (spawnerManager.isCountdownActive) return;

        if (spawnTimer)
        {
            StartCoroutine(SpawnEnemies());
            spawnTimer = false;
        }
    }

    private void CheckIfLevelCompleted()
    {
        if (spawnerManager.enemiesKilledThisRound >= spawnerManager.maxEnemies)
        {

            spawnerManager.isCountdownActive = true;
            StartCoroutine(AdvanceToNextLevel());

        }
    }

    IEnumerator AdvanceToNextLevel()
    {
        spawnerManager.currentLevel += 1;
        spawnerManager.enemiesKilledThisRound = 0;
        spawnerManager.numberOfLivingEnemies = 0;

        if (!spawnerManager.isCountdownActive) spawnerManager.levelCountdown = 0;
        yield return new WaitUntil(() => spawnerManager.levelCountdown >= countdownLength);

        if (spawnerManager.currentLevel < progression.NumberOfLevels())
        {
            spawnerManager.maxEnemies = progression.GetSpawnLevel(spawnerManager.currentLevel) * enemyNumberMultiplier;
        }
        else
        {
            spawnerManager.maxEnemies = progression.GetSpawnLevel((int)progression.NumberOfLevels()) * enemyNumberMultiplier;
        }
        spawnerManager.levelCountdown = 0f;
        spawnerManager.isCountdownActive = false;
    }

    IEnumerator SpawnEnemies()
    {
        GameObject enemyToSpawn = null;
        enemyToSpawn = ChooseUniqueEnemey();

        yield return new WaitForSeconds(spawnFrequency);

        if (!spawnerManager.isCountdownActive && spawnerManager.numberOfLivingEnemies < spawnerManager.maxEnemies)
        {
            
            KeepTrackOfSpawns(enemyToSpawn);
            Instantiate(enemyToSpawn, transform);
            spawnerManager.numberOfLivingEnemies++;
        }
        spawnTimer = true;
    }

    private GameObject ChooseUniqueEnemey()
    {
        GameObject enemyToSpawn;
        if (spawnerManager.lastSpawn != null && spawnerManager.secondToLastSpawn != null)
        {
            if (spawnerManager.lastSpawn == spawnerManager.secondToLastSpawn)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy != spawnerManager.lastSpawn)
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
                enemyToSpawn = enemies[Mathf.RoundToInt(UnityEngine.Random.Range(0, enemies.Length))];
                return enemyToSpawn;
            }
        }
        else
        {
            enemyToSpawn = enemies[Mathf.RoundToInt(UnityEngine.Random.Range(0, enemies.Length))];
            return enemyToSpawn;
        }
        return null;
    }

    private void KeepTrackOfSpawns(GameObject enemy)
    {
        spawnerManager.secondToLastSpawn = spawnerManager.lastSpawn;
        spawnerManager.lastSpawn = enemy;
    }

    public void IncrementKilledThisRound()
    {
        spawnerManager.enemiesKilledThisRound += 1;
    }

    public void decreaseEnemyCount()
    {
        spawnerManager.numberOfLivingEnemies--;
    }
}
