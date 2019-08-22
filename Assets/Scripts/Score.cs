using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] float scoreMultiplier = 1f;
    [SerializeField] TextMeshProUGUI scoreDisplay;
    public float score = 0f;
    PlayerHealth player;
    SpawnerManager sm;

    private void Awake()
    {
        score = 0f;
        player = FindObjectOfType<PlayerHealth>();
        sm = SpawnerManager.Instance;
    }

    void Update()
    {
        scoreDisplay.text = "Score " + "- " + Mathf.RoundToInt(score); 

        if (PauseGameSingleton.Instance.isPaused) return;
        if (player.IsDead()) return;
        if (sm.isCountdownActive) return;

        score += Time.deltaTime * scoreMultiplier * sm.currentLevel;
    }

    public float GetScore()
    {
        return Mathf.RoundToInt(score);
    }
}
