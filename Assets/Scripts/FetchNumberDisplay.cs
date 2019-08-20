using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FetchNumberDisplay : MonoBehaviour
{
    [SerializeField] Texture icon;
    [SerializeField] float xIconSeparation = 2.3f;

    CompanionFetch companionFetch = null;

    float remainingFetchNumber = 0f;

    private void Awake()
    {
        companionFetch = FindObjectOfType<CompanionFetch>();
    }

    private void OnGUI()
    {
        for (int i = 0; i < remainingFetchNumber; i++)
        {
            GUI.DrawTexture(new Rect(20 * (i * xIconSeparation) + 10, 20, 40, 40), icon);
        }
    }

    void Update()
    {
        if (PauseGameSingleton.Instance.isPaused) return;

        if (companionFetch != null)
        {
            remainingFetchNumber = companionFetch.RemainingFetch();          
        }

        if (remainingFetchNumber > 0)
        {
            return;
        }
    }
}
