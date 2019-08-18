using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] zombieRunningSounds;
    [SerializeField] AudioClip[] zombieAttackSounds;
    [SerializeField] AudioClip[] zombieDeathSounds;
    [SerializeField] AudioSource audioSource;

    [SerializeField] float minWait = 1f;
    [SerializeField] float maxWait = 5f;

    float timeSinceLastClip = 0f;
    float timeInterval = 0f;
    Health zombie;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        zombieRunningSounds = Resources.LoadAll<AudioClip>("Running");
        zombieAttackSounds = Resources.LoadAll<AudioClip>("Attack");
        zombieDeathSounds = Resources.LoadAll<AudioClip>("Death");

        zombie = GetComponent<Health>();

        if (timeInterval == 0f) timeInterval = Random.Range(minWait, maxWait);

    }

    void Update()
    {
        timeSinceLastClip += Time.deltaTime;


        var zombieSound = Mathf.RoundToInt(Random.Range(0, zombieRunningSounds.Length));

        if (timeSinceLastClip < timeInterval) return;

        if (zombie.IsDead()) return;
        if (GetComponent<Enemy>().IsAttacking()) return;

        if (!audioSource.isPlaying)
        {
            timeInterval = Random.Range(minWait, maxWait);
            timeSinceLastClip = 0f;
            audioSource.clip = zombieRunningSounds[zombieSound];
            audioSource.Play();
        }
    }

    public void PlayDeathSounds()
    {
        audioSource.Stop();
        var deathSoundIndex = Mathf.RoundToInt(Random.Range(0, zombieDeathSounds.Length));
        audioSource.clip = zombieDeathSounds[deathSoundIndex];
        audioSource.Play();
    }

    public void PlayAttackSounds()
    {
        if (audioSource.isPlaying) return;
        audioSource.Stop();
        var attackSoundIndex = Mathf.RoundToInt(Random.Range(0, zombieAttackSounds.Length));
        audioSource.clip = zombieAttackSounds[attackSoundIndex];
        audioSource.Play();
    }
}
