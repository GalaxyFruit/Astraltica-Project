using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;

    private NavMeshAgent agent;
    private EnemyAnimationController animationController;
    private bool isDead = false;
    private bool isAttacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationController = GetComponent<EnemyAnimationController>();
        agent.speed = patrolSpeed;
    }

    private void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else if (distance <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        UpdateMovementAnimation();
    }

    private void Patrol()
    {
        if (!agent.hasPath)
        {
            Vector3 randomDirection = transform.position + Random.insideUnitSphere * 5f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        agent.speed = patrolSpeed;
        isAttacking = false; 
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
        isAttacking = false; 
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.ResetPath();
            animationController.PlayAttackAnimation();
            Invoke(nameof(ResetAttack), GetAttackDuration());
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;

        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isDead)
        {
            AttackPlayer(); 
        }
    }

    private float GetAttackDuration()
    {
        return animationController.GetCurrentAttackDuration();
    }

    private void UpdateMovementAnimation()
    {
        float velocityMagnitude = agent.velocity.magnitude / agent.speed;
        animationController.SetMoveSpeed(velocityMagnitude);
    }

    public void Kill()
    {
        isDead = true;
        agent.ResetPath();
        animationController.PlayDeathAnimation();
    }
}
