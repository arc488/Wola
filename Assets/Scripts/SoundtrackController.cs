using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackController : MonoBehaviour
{
    [SerializeField] float fadeTime = 2f;
    [SerializeField] AudioClip siren = null;
    [SerializeField] AudioClip soundtrack = null;
    [SerializeField] float soundtrackVolume = 0.7f;
    AudioSource audioSource;
    float currentVolume;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = siren;
        StartCoroutine(PlaySiren());

    }

     IEnumerator PlaySiren()
    {
        var clipLength = audioSource.clip.length;
        audioSource.Play();
        yield return new WaitForSeconds(clipLength - 2);
        StartCoroutine(MyFadeOut());
        yield return new WaitForSeconds(1);
        StartCoroutine(MyFadeIn());
        LoopSoundtrack();
    }

    private void LoopSoundtrack()
    {
        audioSource.clip = soundtrack;
        audioSource.loop = true;
        audioSource.Play();
    }

    IEnumerator MyFadeIn()
    {
        currentVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, soundtrackVolume, elapsedTime / fadeTime);
            yield return null;
        }
    }

    IEnumerator MyFadeOut()
    {
        currentVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / fadeTime);
            yield return null;
        }
    }

}
