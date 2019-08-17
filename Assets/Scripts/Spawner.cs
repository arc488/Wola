using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float maxEnemies = 3;
    [SerializeField] float spawnFrequency = 1f;
    [SerializeField] Progression progression = null;
    [SerializeField] float enemyNumberMultiplier = 10f;
    [SerializeField] float countdownLength = 5f;

    public int currentLevel = 1;
    public int enemiesKilledThisRound = 0;

    public float levelCountdown = 0f;
    public bool isCountdownActive = false;

    public bool spawnTimer = true;
    public static int numberOfEnemies = 0;

    public GameObject lastSpawn = null;
    public GameObject secondToLastSpawn = null;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void Start()
    {
        maxEnemies = progression.GetSpawnLevel(currentLevel) * enemyNumberMultiplier;
    }

    private void Update()
    {

        if (isCountdownActive) levelCountdown += Time.deltaTime;

        CheckIfLevelCompleted();


        if (isCountdownActive) return;

        if (spawnTimer)
        {
            StartCoroutine(SpawnEnemies());
            spawnTimer = false;
        }
    }

    private void CheckIfLevelCompleted()
    {
        if (enemiesKilledThisRound >= maxEnemies)
        {

            isCountdownActive = true;
            StartCoroutine(AdvanceToNextLevel());

        }
    }

    IEnumerator AdvanceToNextLevel()
    {
        currentLevel += 1;
        enemiesKilledThisRound = 0;
        numberOfEnemies = 0;

        if (!isCountdownActive) levelCountdown = 0;
        yield return new WaitUntil(() => levelCountdown >= countdownLength);

        if (currentLevel < progression.NumberOfLevels())
        {
            maxEnemies = progression.GetSpawnLevel(currentLevel) * enemyNumberMultiplier;
        }
        else
        {
            maxEnemies = progression.GetSpawnLevel((int)progression.NumberOfLevels()) * enemyNumberMultiplier;
        }
        levelCountdown = 0f;
        isCountdownActive = false;
    }

    IEnumerator SpawnEnemies()
    {
        GameObject enemyToSpawn = null;
        enemyToSpawn = ChooseUniqueEnemey();

        yield return new WaitForSeconds(spawnFrequency);

        if (!isCountdownActive && numberOfEnemies < maxEnemies)
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
        secondToLastSpawn = lastSpawn;
        lastSpawn = enemy;
    }

    public void IncrementKilledThisRound()
    {
        enemiesKilledThisRound += 1;
    }

    public void decreaseEnemyCount()
    {
        numberOfEnemies--;
    }
}
