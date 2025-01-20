using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float checkTime = 1f;

    private EnemyAnimationController animationController;
    private NavMeshAgent agent;
    private float timer = 0;
    private bool isDead = false;

    private void Start()
    {
        animationController = GetComponent<EnemyAnimationController>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        timer = checkTime;

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
            Idle();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animationController.SetAction(2); // Run
    }

    private void Idle()
    {
        agent.ResetPath();
        animationController.SetAction(0); // Idle
    }

    private void AttackPlayer()
    {
        agent.ResetPath();

        float attackType = Random.Range(3f, 5f); // Left Punch = 3, Right Punch = 4
        animationController.SetAction(attackType);
    }

    public void Kill()
    {
        isDead = true;
        agent.ResetPath();
        animationController.Die();
    }
}
