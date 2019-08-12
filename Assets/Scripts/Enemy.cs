using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float attackRange = 2f;
    [Range(0, 1)]
    [SerializeField] float chaseSpeed = 1f;
    [SerializeField] float speedMultiplier = 1f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] bool chaseOnSpawn = true;


    NavMeshAgent navMeshAgent;
    GameObject player;
    Animator animatorController;
    float maxSpeed;

    public bool isAttacking = false;
    public bool isChasing = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animatorController = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GetComponent<Health>().IsDead()) return;
        if (isAttacking) SlerpTowardTarget(player.transform);
        ChaseSequence();
        ControlAnimation();
        SetMovementSpeed();
        Attack();
    }

    private void SetMovementSpeed()
    {
        float calculatedChaseSpeed = chaseSpeed * speedMultiplier;

        maxSpeed = Mathf.Max(calculatedChaseSpeed);


        navMeshAgent.speed = calculatedChaseSpeed;
        
    }

    private void ControlAnimation()
    {
        var movementSpeed = navMeshAgent.speed / maxSpeed;
        animatorController.SetFloat("speed", movementSpeed);
    }

    private void Attack()
    {
        if (DistanceToPlayer() < attackRange)
        {
            isAttacking = true;
            animatorController.SetTrigger("attack");
        }
        else
        {
            isAttacking = false;
            animatorController.ResetTrigger("attack");
        }
    }

    private void ChaseSequence()
    {
        if (!chaseOnSpawn)
        {
            if (IsWithinRange() && DistanceToPlayer() > attackRange)
            {
                isChasing = true;
                MoveToTarget(player.transform);
            }
            else
            {
                isChasing = false;
            }
        }
        else if (DistanceToPlayer() > attackRange)
        {
            isChasing = true;
            MoveToTarget(player.transform);
        }

        if (navMeshAgent.remainingDistance < attackRange)
        {
            navMeshAgent.isStopped = true;
        }

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void MoveToTarget(Transform target)
    {
        navMeshAgent.isStopped = false;
        SlerpTowardTarget(target);
        navMeshAgent.SetDestination(target.position);
    }

    private void SlerpTowardTarget(Transform target)
    {
        var direction = target.position - transform.position;
        Quaternion lookAt = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotationSpeed);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    public bool IsWithinRange()
    {
        if (Vector3.Distance(navMeshAgent.transform.position, player.transform.position) < chaseDistance)
        {
            return true;
        }
        return false;
    }
}