using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameSingleton : GenericSingletonClass<PauseGameSingleton>
{
    public bool isPaused = true;
}