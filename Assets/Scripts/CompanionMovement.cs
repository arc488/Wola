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

    public float distanceToPlayer = 0f;
    NavMeshAgent navMeshAgent;
    public bool isFetching = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToPlayer = DistanceToPlayer();
        if (isFetching) return;
        if (DistanceToPlayer() < minimumRadius) navMeshAgent.isStopped = true;
        if (DistanceToPlayer() < lingerRadius) return;
        MoveToTarget(player.transform.position);
        

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

    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    public void Fetch(RaycastHit hit)
    {
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
