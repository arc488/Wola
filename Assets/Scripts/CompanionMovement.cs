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
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToPlayer = DistanceToPlayer();
        if (DistanceToPlayer() < minimumRadius) navMeshAgent.isStopped = true;
        if (DistanceToPlayer() < lingerRadius) return;
        MoveToTarget(player.transform);
        

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

}
