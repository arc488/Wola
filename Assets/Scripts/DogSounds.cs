using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] dogBarks;
    [SerializeField] AudioSource audioSource;

    [SerializeField] float minBarkwait = 1f;
    [SerializeField] float maxBarkwait = 5f;

    float timeSinceLastBark = 0f;
    float timeInterval = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (timeInterval == 0f) timeInterval = Random.Range(minBarkwait, maxBarkwait);

    }

    void Update()
    {
        timeSinceLastBark += Time.deltaTime;


        var dogBark = Mathf.RoundToInt(Random.Range(0, dogBarks.Length));

        if (timeSinceLastBark < timeInterval) return;

        if (!audioSource.isPlaying)
        {
            timeInterval = Random.Range(minBarkwait, maxBarkwait);
            timeSinceLastBark = 0f;
            audioSource.clip = dogBarks[dogBark];
            audioSource.Play();
        }
    }
}
