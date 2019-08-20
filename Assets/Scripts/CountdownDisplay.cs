using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownDisplay : MonoBehaviour
{
    [SerializeField] Canvas countdownCanvas = null;
    [SerializeField] TextMeshProUGUI countdownDisplay;
    [SerializeField] AudioClip countdownClip = null;
    [SerializeField] float startCountdown = 10f;

    [SerializeField] AudioClip[] countdown;

    SpawnerManager sm;
    AudioSource audioSource;
    float currentCountdownTime;
    float countdownDuration;
    bool keepPlaying = false;


    private void Awake()
    {
        countdownDisplay.text = startCountdown.ToString();
        sm = SpawnerManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = countdownClip;
        countdownCanvas.enabled = false;
        countdownDisplay.enabled = false;

    }

    void Update()
    {

        if (sm.isCountdownActive)
        {
            StartCoroutine(PlayCountDownClip());
            countdownCanvas.enabled = true;
            countdownDisplay.enabled = true;
            countdownDisplay.text = (startCountdown - Mathf.RoundToInt(sm.levelCountdown)).ToString();
        }
        else
        {
            countdownDisplay.text = startCountdown.ToString();
            countdownDisplay.enabled = false;
            countdownCanvas.enabled = false;
        }

    }

    IEnumerator PlayCountDownClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = countdown[int.Parse(countdownDisplay.text)];
            audioSource.Play();
            yield return null;
        }
    }

    public float GetClipLength()
    {
        return countdownClip.length;
    }
}
