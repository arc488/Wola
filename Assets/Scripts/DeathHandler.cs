using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas deathCanvas;
    [SerializeField] TextMeshProUGUI display;
    Score score;
    PlayerHealth player;
    bool deathSequenceInitialized = false;
    private void Awake()
    {
        score = FindObjectOfType<Score>();
        deathCanvas.enabled = false;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        if (deathSequenceInitialized) return;
        if (player.IsDead())
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        PauseGameSingleton.Instance.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathCanvas.enabled = true;
        display.text = score.GetScore().ToString();
        Debug.LogAssertion(Cursor.visible);
        Debug.LogAssertion(Cursor.lockState);
        deathSequenceInitialized = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
