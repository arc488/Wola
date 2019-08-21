using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas deathCanvas;

    PlayerHealth player;
    bool deathSequenceInitialized = false;
    private void Awake()
    {
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
