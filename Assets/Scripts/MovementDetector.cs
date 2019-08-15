using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDetector : MonoBehaviour
{

    [SerializeField] AutomaticLight lightSet = null;



    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (lightSet == null)
        {
            Debug.LogWarning("No lightSet attached");
            return;
        }

        lightSet.TurnOn();

    }
}
