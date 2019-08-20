using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas deathCanvas;

    PlayerHealth player;

    private void Awake()
    {
        deathCanvas.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        if (player.IsDead())
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        PauseGameSingleton.Instance.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        deathCanvas.enabled = true;
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
