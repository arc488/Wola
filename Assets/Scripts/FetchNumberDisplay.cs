using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FetchNumberDisplay : MonoBehaviour
{
    CompanionFetch companionFetch = null;

    private void Awake()
    {
        companionFetch = FindObjectOfType<CompanionFetch>();
    }

    void Update()
    {
        if (companionFetch != null)
        {
            GetComponent<TextMeshProUGUI>().text = companionFetch.RemainingFetch().ToString();          
        }
    }
}
