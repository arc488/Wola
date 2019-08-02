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

    public void TakeDamge(float damage)
    {
        health -= damage;
    }

    private void Start()
    {
        if (this.tag == "Enemy")
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
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

    void Die()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.stoppingDistance = 0f;
        navMeshAgent.isStopped = true;
        if (animator != null)
        {
            animator.SetTrigger("die");           
        }

    }

    public bool IsDead()
    {
        return isDead;
    }
}
