using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticLight : MonoBehaviour
{

    [SerializeField] float lightRange = 10f;
    [SerializeField] float stayOnFor = 5f;

    public float timeOn;
    Light[] lights;
    bool lightsOn = false;


    void Start()
    {
        lights = transform.GetComponentsInChildren<Light>();
        TurnOff();
    }

    void Update()
    {
        if (lightsOn)
        {
            timeOn += Time.deltaTime;
        }

        if (timeOn > stayOnFor)
        {
            TurnOff();
        }
    }

    public void TurnOff()
    {
        foreach (Light light in lights)
        {
            lightsOn = false;
            light.range = 0;
        }
    }

    public void TurnOn()
    {
        timeOn = 0f;
        foreach (Light light in lights)
        {
            lightsOn = true;
            light.range = lightRange;
        }
    }



}
