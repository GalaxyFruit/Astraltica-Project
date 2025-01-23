using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float maxTimer = 0.1f; 

    private NavMeshAgent agent;
    private EnemyAnimationController animationController;
    private float timer;
    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationController = GetComponent<EnemyAnimationController>();
        agent.speed = patrolSpeed;
        timer = maxTimer;

        StartCoroutine(StateCheck());
    }
    private void Update()
    {
        if (isDead) return;

        UpdateMovementAnimation(); // Aktualizujeme animaci pohybu každý snímek
    }


    private IEnumerator StateCheck()
    {
        while (!isDead) 
        {
            float sqrDistance = (player.position - transform.position).sqrMagnitude;

            if (sqrDistance <= attackRange * attackRange)
            {
                if (IsPathValid())
                {
                    AttackPlayer();
                }
                else
                {
                    Idle();
                }
            }
            else if (sqrDistance <= detectionRange * detectionRange)
            {
                if (IsPathValid())
                {
                    ChasePlayer();
                }
                else
                {
                    Idle();
                }
            }
            else
            {
                Patrol();
            }

            yield return new WaitForSeconds(0.1f); // Provádíme kontrolu každých 0.1 sekundy
        }
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
    }

    private void Idle()
    {
        Debug.Log("Enemy is idle - no valid path to the player!");
        agent.ResetPath();
        animationController.SetMoveSpeed(0); 
    }

    private bool IsPathValid()
    {
        return agent.hasPath && !agent.pathPending && agent.pathStatus == NavMeshPathStatus.PathComplete;
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position); 
        agent.speed = chaseSpeed; 
    }

    private void AttackPlayer()
    {
        if (!animationController.IsAttacking()) 
        {
            Debug.Log("Enemy is attacking!");
            agent.ResetPath();
            animationController.PlayAttackAnimation(); 
            timer = GetAttackDuration(); 
        }
    }

    private float GetAttackDuration()
    {
        return animationController.GetCurrentAttackDuration(); 
    }

    private void UpdateMovementAnimation()
    {
        animationController.SetMoveSpeed(agent.velocity.magnitude / agent.speed); 
    }

    public void Kill()
    {
        isDead = true; 
        agent.ResetPath(); 
        animationController.PlayDeathAnimation(); 
    }
}
