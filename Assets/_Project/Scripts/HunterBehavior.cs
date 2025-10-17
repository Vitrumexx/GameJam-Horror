using UnityEngine;
using UnityEngine.AI;

public class HunterBehavior : MonoBehaviour
{
    public float chaseDistance = 30f;
    public float attackDistance = 10f;
    public float rotationSpeed = 10f;

    public Transform[] patrolPoints;
    public Animator animator;

    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing;
    private int currentPatrolIndex;


    public Transform modelRoot;

    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Управляем вращением вручную
        agent.updateRotation = false;
        agent.updatePosition = true;

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
        }
        else
        {
            Patrol();
        }

        RotateTowardsMovementDirection();

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Patrol()
    {
        agent.speed = 3.5f;
        isChasing = false;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Chase()
    {
        agent.speed = 5f;
        isChasing = true;
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);
        animator.SetTrigger("Attack");
    }

    void RotateTowardsMovementDirection()
    {
        Vector3 direction;

        if (agent.hasPath && agent.remainingDistance > 0.1f)
            direction = (agent.steeringTarget - transform.position).normalized;
        else
            direction = agent.velocity.normalized;

        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}