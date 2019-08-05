using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int maxEnemies = 3;
    [SerializeField] float spawnFrequency = 1f;

    public bool spawnTimer = true;
    public int numberOfEnemies = 0;

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
        yield return new WaitForSeconds(spawnFrequency);
        if (numberOfEnemies < maxEnemies)
        {
            GameObject enemy = enemies[Mathf.RoundToInt(Random.Range(0, enemies.Length - 1))];
            Instantiate(enemy, transform);
            numberOfEnemies++;
        }
        spawnTimer = true;
    }
}
