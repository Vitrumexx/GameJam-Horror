using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HunterBehavior : MonoBehaviour
{

    public float chaseDistance = 30f;
    public float attackDistance = 10f;
    public float rotationSpeed = 50f;
    private int currentPatrolIndex;

    public Transform[] patrolPoints;
    public Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private Transform target;

    private Vector3 originalVelocity;
    private bool isChasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackDistance)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            Chase();
            FaceTarget(player);
        }
        else
        {
            Patrol();
            if (currentPatrolIndex > 0)
                FaceTarget(patrolPoints[currentPatrolIndex - 1].transform);
            else
                FaceTarget(patrolPoints[patrolPoints.Length - 1].transform);
        }

        animator.SetFloat("Speed", agent.speed);
    }

    void Patrol()
    {
        agent.speed = 2f;
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            GoToNextPatrolPoint();
        }


    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        agent.speed = 4f;
        if (!isChasing)
        {
            isChasing = true;
        }
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);
        animator.SetTrigger("Attack");
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
