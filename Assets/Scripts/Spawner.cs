using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int maxEnemies = 3;
    public int numberOfEnemies = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void Update()
    {
        if (numberOfEnemies < maxEnemies)
        {
            GameObject enemy = enemies[Mathf.RoundToInt(Random.Range(0, enemies.Length - 1))];
            Instantiate(enemy, transform);
            numberOfEnemies++;
        }
    }
}
