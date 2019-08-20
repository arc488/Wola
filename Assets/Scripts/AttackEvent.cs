using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    PlayerHealth player;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        player = FindObjectOfType<PlayerHealth>();
    }

    void HitEvent()
    {
        print("Hit event");
        enemy.AttackEvent();
    }


}
