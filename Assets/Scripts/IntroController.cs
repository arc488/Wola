using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] Canvas introCanvas;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PauseGameSingleton.Instance.isPaused = false;
            introCanvas.enabled = false;
            
        }   
    }
}
