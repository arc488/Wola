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
    [Range(0, 1)]
    [SerializeField] float lowerSpeedLimit = 0.5f;
    [Range(0, 1)]
    [SerializeField] float upperSpeedLimit = 1f;
    [SerializeField] float damage = 2f;


    NavMeshAgent navMeshAgent;
    GameObject player;
    Animator animatorController;
    float maxSpeed;
    CompanionMovement companion;
    GameObject m_Target;
    ZombieSounds zs;
    AttackEvent attackEvent;

    public bool isAttacking = false;
    public bool isChasing = false;

    void Start()
    {
        attackEvent = GetComponentInChildren<AttackEvent>();
        companion = FindObjectOfType<CompanionMovement>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animatorController = GetComponentInChildren<Animator>();
        zs = GetComponent<ZombieSounds>();

        RandomChaseSpeed();
    }



    void Update()
    {
        if (PauseGameSingleton.Instance.isPaused) return;
        if (GetComponent<Health>().IsDead()) return;
        m_Target = ChooseTarget();
        if (isAttacking) SlerpTowardTarget(m_Target.transform);
        ChaseSequence();
        ControlAnimation();
        SetMovementSpeed();
        Attack();
    }

    private void RandomChaseSpeed()
    {
        chaseSpeed = UnityEngine.Random.Range(lowerSpeedLimit, upperSpeedLimit);
    }

    private GameObject ChooseTarget()
    {
        if (companion.IsFetching())
        {
            return companion.gameObject;
        }
        else
        {
            return player;
        }
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
        if (DistanceToTarget(m_Target.transform) < attackRange)
        {
            isAttacking = true;
            zs.PlayAttackSounds();
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
            if (IsWithinRange(m_Target.transform) && DistanceToTarget(m_Target.transform) > attackRange)
            {
                isChasing = true;
                MoveToTarget(m_Target.transform);
            }
            else
            {
                isChasing = false;
            }
        }
        else if (DistanceToTarget(m_Target.transform) > attackRange)
        {
            isChasing = true;
            MoveToTarget(m_Target.transform);
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

    private float DistanceToTarget(Transform target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }

    public bool IsWithinRange(Transform target)
    {
        if (Vector3.Distance(navMeshAgent.transform.position, target.position) < chaseDistance)
        {
            return true;
        }
        return false;
    }

    public void AttackEvent()
    {
        if (isAttacking)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}