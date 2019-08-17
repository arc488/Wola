using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Progression/New Progression Chart", order = 1)]
public class Progression : ScriptableObject
{
    [SerializeField] float[] spawnLevels;

    public float GetSpawnLevel(int level)
    {
        return spawnLevels[level-1];
    }

    public float NumberOfLevels()
    {
        return spawnLevels.Length;
    }
}