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
    [SerializeField] float fetchTolerance = 0.5f;
    [SerializeField] float fetchWaitTime = 3f;

    public float distanceToPlayer = 0f;
    NavMeshAgent navMeshAgent;
    public bool isFetching = false;
    public bool isWaiting = false;
    Vector3 fetchPosition = Vector3.zero;
    Animator animatorController;

    //Player avoidance variables
    Vector3 avoidDirection;
    Vector3 avoidDestination;
    public bool isAvoding = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponentInChildren<Animator>();

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(avoidDirection, 0.3f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(avoidDestination, 0.3f);
    }


    void Update()
    {
        if (PauseGameSingleton.Instance.isPaused) return;

        distanceToPlayer = DistanceToTarget(player.transform.position);
        ControlAnimation();

        if (fetchPosition != Vector3.zero)
        {
            StartCoroutine(HasReachedFetchPosition());
        }


        if (isWaiting) LookAtNearestEnemy();
        if (isFetching) return;

        if (DistanceToTarget(avoidDestination) < 1.5f) isAvoding = false;

        if (DistanceToTarget(player.transform.position) < minimumRadius && !isAvoding) CompanionWait();

        if (DistanceToTarget(player.transform.position) < lingerRadius)
        {
            return;
        }
        isWaiting = false;
        navMeshAgent.speed = speed;
        MoveToTarget(player.transform.position);
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        avoidDirection = collision.contacts[0].point - transform.position;
        avoidDirection = avoidDirection.normalized;

        avoidDestination = transform.position - avoidDirection * 2;
        avoidDestination.y = transform.position.y;

        isAvoding = true;

        MoveToTarget(avoidDestination);

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
        isWaiting = false;
        navMeshAgent.speed = speed;
        fetchPosition = hit.point;
        isFetching = true;
        MoveToTarget(hit.point);
    }

    public void MoveToTarget(Vector3 position)
    {
        isWaiting = false;
        navMeshAgent.speed = speed;
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
