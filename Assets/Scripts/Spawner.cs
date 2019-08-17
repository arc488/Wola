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

    public SpawnerManager sm;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void Start()
    {
        sm = SpawnerManager.Instance;
        sm.maxEnemies = progression.GetSpawnLevel(sm.currentLevel) * enemyNumberMultiplier;
    }

    private void Update()
    {

        if (sm.isCountdownActive) sm.levelCountdown += Time.deltaTime;

        CheckIfLevelCompleted();


        if (sm.isCountdownActive) return;

        if (spawnTimer)
        {
            StartCoroutine(SpawnEnemies());
            spawnTimer = false;
        }
    }

    private void CheckIfLevelCompleted()
    {
        if (sm.enemiesKilledThisRound >= sm.maxEnemies)
        {

            sm.isCountdownActive = true;
            StartCoroutine(AdvanceToNextLevel());

        }
    }

    IEnumerator AdvanceToNextLevel()
    {
        sm.currentLevel += 1;
        sm.enemiesKilledThisRound = 0;
        sm.numberOfLivingEnemies = 0;

        if (!sm.isCountdownActive) sm.levelCountdown = 0;
        yield return new WaitUntil(() => sm.levelCountdown >= countdownLength);

        if (sm.currentLevel < progression.NumberOfLevels())
        {
            sm.maxEnemies = progression.GetSpawnLevel(sm.currentLevel) * enemyNumberMultiplier;
        }
        else
        {
            sm.maxEnemies = progression.GetSpawnLevel((int)progression.NumberOfLevels()) * enemyNumberMultiplier;
        }
        sm.levelCountdown = 0f;
        sm.isCountdownActive = false;
    }

    IEnumerator SpawnEnemies()
    {
        GameObject enemyToSpawn = null;
        enemyToSpawn = ChooseUniqueEnemey();

        yield return new WaitForSeconds(spawnFrequency);

        if (!sm.isCountdownActive && sm.numberOfLivingEnemies < sm.maxEnemies)
        {
            
            KeepTrackOfSpawns(enemyToSpawn);
            Instantiate(enemyToSpawn, transform);
            sm.numberOfLivingEnemies++;
        }
        spawnTimer = true;
    }

    private GameObject ChooseUniqueEnemey()
    {
        GameObject enemyToSpawn = null;
        if (sm.lastSpawn == null)
        {
            enemyToSpawn = enemies[Mathf.RoundToInt(UnityEngine.Random.Range(0, enemies.Length))];
            return enemyToSpawn;
        }
        if (sm.lastSpawn != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != sm.lastSpawn)
                {
                    enemyToSpawn = enemy;
                    return enemy;
                }
            }
        }
        return null;
    }

    private void KeepTrackOfSpawns(GameObject enemy)
    {
        sm.lastSpawn = enemy;
    }

    public void IncrementKilledThisRound()
    {
        sm.enemiesKilledThisRound += 1;
    }

    public void decreaseEnemyCount()
    {
        sm.numberOfLivingEnemies--;
    }
}
