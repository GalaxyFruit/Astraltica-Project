using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stateUpdateInterval = 0.1f;
    [SerializeField] private float minWaitTimer = 3f;
    [SerializeField] private float maxWaitTimer = 10f;

    private NavMeshAgent agent;
    private EnemyAnimationController animationController;

    private IEnemyState currentState;
    private float stateUpdateTimer;
    private bool isDead;

    public Transform Player => player;
    public NavMeshAgent Agent => agent;
    public EnemyAnimationController AnimationController => animationController;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public float PatrolSpeed => patrolSpeed;
    public float PatrolRadius => patrolRadius;
    public float ChaseSpeed => chaseSpeed;
    public float StateUpdateInterval => stateUpdateInterval;
    public float MinWaitTimer => minWaitTimer;
    public float MaxWaitTimer => maxWaitTimer;
    public bool IsDead => isDead;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationController = GetComponent<EnemyAnimationController>();
        stateUpdateTimer = stateUpdateInterval;

        SwitchState(new PatrolState(this));
        GetComponent<EnemyHealth>().OnDeath += HandleDeath;
    }

    private void Update()
    {
        stateUpdateTimer -= Time.deltaTime;
        if (stateUpdateTimer <= 0f)
        {
            currentState?.UpdateState();
            stateUpdateTimer = stateUpdateInterval;
        }

        UpdateMovementAnimation();
    }

    public void SwitchState(IEnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    public bool IsPlayerInRange(float range)
    {
        return Vector3.SqrMagnitude(player.position - transform.position) <= range * range;
    }

    public void UpdateMovementAnimation()
    {
        animationController.SetMoveSpeed(agent.velocity.magnitude / agent.speed);
    }

    private void HandleDeath()
    {
        isDead = true;
        agent.ResetPath();
        currentState = null;
    }
}
