using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FetchNumberDisplay : MonoBehaviour
{
    [SerializeField] Texture icon;
    [SerializeField] float xIconSeparation = 2.3f;
    [SerializeField] GameObject[] icons;
    CompanionFetch companionFetch = null;
    

    public float remainingFetchNumber = 0f;

    private void Awake()
    {
        companionFetch = FindObjectOfType<CompanionFetch>();
        remainingFetchNumber = companionFetch.RemainingFetch();
        icons = GameObject.FindGameObjectsWithTag("FetchIcon");

        foreach (var icon in icons)
        {
            icon.SetActive(false);
        }
    }


    void Update()
    {
        if (PauseGameSingleton.Instance.isPaused) return;

        if (companionFetch != null)
        {
            remainingFetchNumber = companionFetch.RemainingFetch();          
        }

        //if (remainingFetchNumber > 0)
        //{
        //return;
        //}

        foreach (GameObject icon in icons)
        {
            icon.SetActive(false);
        }

        if (remainingFetchNumber <= 0) return;

        for (int i = 0; i < remainingFetchNumber; i++)
        {
            print(i);
            icons[i].SetActive(true);
        }
    }
}
