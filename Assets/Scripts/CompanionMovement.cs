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

    public float distanceToPlayer = 0f;
    NavMeshAgent navMeshAgent;
    public bool isFetching = false;
    Vector3 fetchPosition = Vector3.zero;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToPlayer = DistanceToTarget(player.transform.position);
        if (fetchPosition != Vector3.zero)
        {
            HasReachedFetchPosition();
        }
        if (isFetching) return;
        if (DistanceToTarget(player.transform.position) < minimumRadius) navMeshAgent.isStopped = true;
        if (DistanceToTarget(player.transform.position) < lingerRadius) return;
        MoveToTarget(player.transform.position);
        

    }

    private void HasReachedFetchPosition()
    {
        if (DistanceToTarget(fetchPosition) < fetchTolerance)
        {
            fetchPosition = Vector3.zero;
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

    public float DistanceToTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    public void Fetch(RaycastHit hit)
    {
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

}
