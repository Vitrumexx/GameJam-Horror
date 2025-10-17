using _Project.Scripts.Features.Finders.FieldOfView;
using UnityEngine;
using UnityEngine.AI;

public class HunterBehavior : MonoBehaviour
{
    [Header("Distances")]
    [SerializeField] private float chaseDistance = 30f;
    [SerializeField] private float attackDistance = 10f;

    [Header("Speeds")]
    [SerializeField] private float patrolSpeed = 3.5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPatrolPoint = 2f;

    [Header("Memory Settings")]
    [SerializeField] private float memoryTime = 3f;

    [Header("Stun Settings")]
    [SerializeField] private float stunDuration = 2f;

    [Header("Components")]
    [SerializeField] private FieldOfViewFinder finder;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform modelRoot;
    [SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject defeatScreen;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 lastKnownPlayerPosition;
    private float waitTimer;
    private float memoryTimer;
    private float stunTimer;
    private int currentPatrolIndex;
    private bool isChasing;
    private bool isWaiting;

    private enum State { Patrol, Chase, Attack, Investigate, Stunned }
    private State currentState;

    private void Start()
    {
        finder.viewRadius = chaseDistance;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (finder == null)
        {
            Debug.LogWarning($"FieldOfViewFinder not assigned on {gameObject.name}!");
        }

        if (animator == null)
        {
            Debug.LogWarning($"Animator not assigned on {gameObject.name}!");
        }

        if (modelRoot == null)
        {
            modelRoot = transform;
        }

        agent.updateRotation = false;
        agent.updatePosition = true;

        currentState = State.Patrol;
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (currentState == State.Stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                currentState = State.Patrol;
                agent.enabled = true;
                GoToNextPatrolPoint();
            }
        }
        else
        {
            switch (currentState)
            {
                case State.Patrol:
                    Patrol();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.Investigate:
                    Investigate();
                    break;
            }

            RotateTowardsMovementDirection();
        }

        UpdateAnimator();
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                GoToNextPatrolPoint();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isWaiting = true;
            waitTimer = waitTimeAtPatrolPoint;
            agent.SetDestination(transform.position);
        }

        if (finder != null && finder.CanSeeTarget && finder.VisibleTarget != null && finder.VisibleTarget.transform == player)
        {
            lastKnownPlayerPosition = player.position;
            memoryTimer = memoryTime;
            currentState = State.Chase;
            isChasing = true;
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, patrolPoints.Length);
        } while (newIndex == currentPatrolIndex && patrolPoints.Length > 1);

        currentPatrolIndex = newIndex;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void Chase()
    {
        agent.speed = chaseSpeed;

        if (finder != null && finder.CanSeeTarget && finder.VisibleTarget != null && finder.VisibleTarget.transform == player)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            lastKnownPlayerPosition = player.position;
            memoryTimer = memoryTime;

            if (distanceToPlayer <= attackDistance)
            {
                currentState = State.Attack;
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
        else
        {
            memoryTimer -= Time.deltaTime;
            if (memoryTimer <= 0)
            {
                currentState = State.Investigate;
                agent.SetDestination(lastKnownPlayerPosition);
            }
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        if (finder != null && finder.CanSeeTarget && finder.VisibleTarget != null && finder.VisibleTarget.transform == player)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            lastKnownPlayerPosition = player.position;
            memoryTimer = memoryTime;

            if (distanceToPlayer > attackDistance)
            {
                currentState = State.Chase;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playerUi.SetActive(false);
                defeatScreen.SetActive(true);
            }
        }
        else
        {
            memoryTimer -= Time.deltaTime;
            if (memoryTimer <= 0)
            {
                currentState = State.Investigate;
                agent.SetDestination(lastKnownPlayerPosition);
            }
        }
    }

    private void Investigate()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
            isChasing = false;
            GoToNextPatrolPoint();
        }
        else if (finder != null && finder.CanSeeTarget && finder.VisibleTarget != null && finder.VisibleTarget.transform == player)
        {
            lastKnownPlayerPosition = player.position;
            memoryTimer = memoryTime;
            currentState = State.Chase;
        }
    }

    private void RotateTowardsMovementDirection()
    {
        Vector3 direction;

        if (currentState == State.Attack && finder != null && finder.CanSeeTarget && finder.VisibleTarget != null && finder.VisibleTarget.transform == player)
        {
            direction = (player.position - transform.position).normalized;
        }
        else if (agent.hasPath && agent.remainingDistance > 0.1f)
        {
            direction = (agent.steeringTarget - transform.position).normalized;
        }
        else
        {
            direction = agent.velocity.normalized;
        }

        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", currentState == State.Stunned ? 0 : agent.velocity.magnitude);
        // animator.SetBool("IsChasing", isChasing && currentState != State.Stunned);
        // animator.SetBool("IsAttacking", currentState == State.Attack);
        // animator.SetBool("IsStunned", currentState == State.Stunned);
    }

    public void Stun(float duration)
    {
        stunTimer = duration > 0 ? duration : stunDuration;
        currentState = State.Stunned;
        agent.enabled = false;
        isChasing = false;
    }
}