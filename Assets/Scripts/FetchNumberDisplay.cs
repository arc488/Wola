using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class FetchNumberDisplay : MonoBehaviour
{
    [SerializeField] Texture icon;
    [SerializeField] GameObject[] icons;
    CompanionFetch companionFetch = null;
    

    public float remainingFetchNumber = 0f;

    private void Awake()
    {
        companionFetch = FindObjectOfType<CompanionFetch>();
        remainingFetchNumber = companionFetch.RemainingFetch();
        icons = GameObject.FindGameObjectsWithTag("FetchIcon");

        Array.Sort(icons, XPositionComparison);

        foreach (var icon in icons)
        {
            icon.SetActive(false);
        }
    }

    private int XPositionComparison(GameObject a, GameObject b)
    {
        var xa = a.transform.position.x;
        var xb = b.transform.position.x;
        return xb.CompareTo(xa); 
    }


    void Update()
    {
        if (PauseGameSingleton.Instance.isPaused) return;

        if (companionFetch != null)
        {
            remainingFetchNumber = companionFetch.RemainingFetch();          
        }

        foreach (GameObject icon in icons)
        {
            icon.SetActive(false);
        }

        if (remainingFetchNumber <= 0) return;

        for (int i = 0; i < remainingFetchNumber; i++)
        {
            icons[i].SetActive(true);
        }
    }
}
