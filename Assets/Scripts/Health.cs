using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;

    bool isDead = false;
    Animator animator;
    NavMeshAgent navMeshAgent;
    Spawner spawner;


    private void Start()
    {
        if (this.tag == "Enemy")
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            spawner = GameObject.FindObjectOfType<Spawner>();
        }
    }

    private void Update()
    {
        if (health <= 0 && isDead == false)
        {
            isDead = true;
            Die();
        }       
    }

    public void TakeDamge(float damage)
    {
        health -= damage;
    }

    void Die()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.stoppingDistance = 0f;
        navMeshAgent.isStopped = true;
        if (animator != null)
        {
            animator.applyRootMotion = true;
            animator.SetTrigger("die");           
        }
        spawner.decreaseEnemyCount();


    }

    public bool IsDead()
    {
        return isDead;
    }
}
