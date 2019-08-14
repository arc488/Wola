using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionMovement : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    [SerializeField] float lingerRadius = 5f;
    [SerializeField] float minimumRadius = 3f;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float fetchTolerance = 2f;
    [SerializeField] float fetchWaitTime = 3f;

    public float distanceToPlayer = 0f;
    NavMeshAgent navMeshAgent;
    bool isFetching = false;
    bool isWaiting = false;
    Vector3 fetchPosition = Vector3.zero;
    Animator animatorController;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        distanceToPlayer = DistanceToTarget(player.transform.position);
        ControlAnimation();
        if (fetchPosition != Vector3.zero)
        {
            StartCoroutine(HasReachedFetchPosition());
        }
        if (isWaiting) LookAtNearestEnemy();
        if (isFetching) return;

        if (DistanceToTarget(player.transform.position) < minimumRadius) CompanionWait();

        if (DistanceToTarget(player.transform.position) < lingerRadius)
        {
            return;
        }
        isWaiting = false;
        navMeshAgent.speed = speed;
        MoveToTarget(player.transform.position);
        

    }

    private void LookAtNearestEnemy()
    {
        Transform nearestEnemy = NearestEnemy();
        if (nearestEnemy == null) return;
        SlerpTowardTarget(nearestEnemy.position);
    }

    private Transform NearestEnemy()
    {
        Transform closestEnemy = null;
        float minimumDist = Mathf.Infinity;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies == null) return null;
        foreach (Enemy enemy in enemies)
        {
            if (DistanceToTarget(enemy.transform.position) < minimumDist && !enemy.GetComponent<Health>().IsDead())
            {
                minimumDist = DistanceToTarget(enemy.transform.position);
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void ControlAnimation()
    {
        var movementSpeed = navMeshAgent.speed / speed;
        animatorController.SetFloat("speed", movementSpeed);
    }

    IEnumerator HasReachedFetchPosition()
    {
        if (DistanceToTarget(fetchPosition) < fetchTolerance)
        {
            fetchPosition = Vector3.zero;
            CompanionWait();
            yield return new WaitForSeconds(fetchWaitTime);
            isWaiting = false;
            isFetching = false;
        }
        else
        {
            isFetching = true;
        }
    }

    public bool CanReachTarget(RaycastHit hit)
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(hit.point, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CompanionWait()
    {
        isWaiting = true;
        navMeshAgent.speed = 0f;
        navMeshAgent.isStopped = true;
    }

    public float DistanceToTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    public void Fetch(RaycastHit hit)
    {
        navMeshAgent.speed = speed;
        fetchPosition = hit.point;
        isFetching = true;
        MoveToTarget(hit.point);
    }

    public void MoveToTarget(Vector3 position)
    {
        navMeshAgent.isStopped = false;
        SlerpTowardTarget(position);
        navMeshAgent.SetDestination(position);
    }

    private void SlerpTowardTarget(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        Quaternion lookAt = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotationSpeed);
    }

    public bool IsFetching()
    {
        return isFetching;
    }

}
