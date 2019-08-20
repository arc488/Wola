using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    PlayerHealth player;
    Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        player = FindObjectOfType<PlayerHealth>();
    }

    void HitEvent()
    {
        enemy.AttackEvent();
    }


}
