﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] Path path;
    [Range(0, 1)]
    [SerializeField] float chaseSpeed = 1f;
    [Range(0, 1)]
    [SerializeField] float patrolSpeed = 0.5f;
    [SerializeField] float speedMultiplier = 1f;
    [SerializeField] float rotationSpeed = 1f;


    Waypoint[] waypoints;
    NavMeshAgent navMeshAgent;
    GameObject player;
    Animator animatorController;
    int curWaypoint;
    float maxSpeed;

    public bool isAttacking = false;
    public bool isChasing = false;
    public bool isPatrolling = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animatorController = GetComponentInChildren<Animator>();
        waypoints = path.GetComponentsInChildren<Waypoint>();
    }

    void Update()
    {
        if (GetComponent<Health>().IsDead()) return;
        Patrol();
        ChaseSequence();
        ControlAnimation();
        SetMovementSpeed();
        Attack();
    }

    private void SetMovementSpeed()
    {
        float calculatedChaseSpeed = chaseSpeed * speedMultiplier;
        float calculatedPatrolSpeed = patrolSpeed * speedMultiplier;

        maxSpeed = Mathf.Max(calculatedChaseSpeed, calculatedPatrolSpeed);

        if (isPatrolling)
        {
            navMeshAgent.speed = calculatedPatrolSpeed;
        }
        else if (isChasing)
        {
            navMeshAgent.speed = calculatedChaseSpeed;
        }
    }

    private void Patrol()
    {
        if (!isAttacking && !isChasing)
        {
            isPatrolling = true;
            MoveToNextWaypoint();
            IncrementWaypoint();
        }
        else
        {
            isPatrolling = false;
        }

    }

    private void IncrementWaypoint()
    {
        if (Vector3.Distance(waypoints[curWaypoint].transform.position, transform.position) < path.GetWaypointTolerance())
        {
            curWaypoint += 1;
        }
    }

    private void MoveToNextWaypoint()
    {
        if (curWaypoint < waypoints.Length)
        {
            MoveToTarget(waypoints[curWaypoint].transform);
        }
        else
        {
            curWaypoint = 0;
        }
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
        if (IsWithinRange() && DistanceToPlayer() > attackRange)
        {
            isChasing = true;
            MoveToTarget(player.transform);
        }
        else
        {
            isChasing = false;
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
        navMeshAgent.transform.LookAt(target);

        //transform.rotation = Quaternion.LookRotation(transform.forward * rotationSpeed, transform.up * rotationSpeed);
        //Quaternion.RotateTowards(transform.rotation, target.rotation, rotationSpeed);
        navMeshAgent.SetDestination(target.position);
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