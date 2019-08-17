using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : GenericSingletonClass<SpawnerManager> 
{
    public float maxEnemies = 3;

    public float existingEnemies = 0f;

    public int currentLevel = 1;
    public int enemiesKilledThisRound = 0;

    public float levelCountdown = 0f;
    public bool isCountdownActive = false;

    public int numberOfLivingEnemies = 0;

    public GameObject lastSpawn = null;
}