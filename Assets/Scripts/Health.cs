﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;

    [SerializeField] int headshotsToDie = 2;
    [SerializeField] AudioClip headshotSound;
    [SerializeField] GameObject head;

    public int headshotCount = 0;

    bool isDead = false;
    bool wasHeadshot = false;

    Animator animator;
    NavMeshAgent navMeshAgent;
    Spawner spawner;
    Collider capsuleCollider;
    NavMeshObstacle obstacle;
    AudioSource audioSource;
    ZombieSounds zombieSounds;


    private void Start()
    {
        if (this.tag == "Enemy")
        {
            audioSource = GetComponent<AudioSource>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            spawner = GameObject.FindObjectOfType<Spawner>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            obstacle = GetComponent<NavMeshObstacle>();
            zombieSounds = GetComponent<ZombieSounds>();
        }
    }

    private void Update()
    {
        if (headshotCount >= headshotsToDie && isDead == false)
        {
            isDead = true;
            wasHeadshot = true;
            health = 0;
            Destroy(head);
            audioSource.clip = headshotSound;
            audioSource.Play();
            Die();
        }

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
        if (!wasHeadshot) zombieSounds.PlayDeathSounds();
        capsuleCollider.enabled = false;
        obstacle.enabled = false;
        spawner.IncrementKilledThisRound();
        Destroy(gameObject, 5f);


    }

    public void Headshot(float damage)
    {
        health -= damage;
        headshotCount += 1;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
