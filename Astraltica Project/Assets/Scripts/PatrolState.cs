using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IEnemyState
{
    private EnemyAI enemy;
    private bool isWaiting;
    private float waitTimer;

    public PatrolState(EnemyAI enemyAI)
    {
        enemy = enemyAI;
    }

    public void EnterState()
    {
        enemy.Agent.speed = enemy.PatrolSpeed;
        MoveToRandomPoint();
    }

    public void UpdateState()
    {
        if (enemy.IsPlayerInRange(enemy.DetectionRange))
        {
            enemy.SwitchState(new ChaseState(enemy));
        }
        else if (!enemy.Agent.hasPath && !isWaiting)
        {
            StartWait();
        }

        if (isWaiting)
        {
            waitTimer -= enemy.StateUpdateInterval;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                MoveToRandomPoint();
            }
        }

        enemy.UpdateMovementAnimation();
    }

    public void ExitState()
    {
        enemy.Agent.ResetPath();
    }

    private void MoveToRandomPoint()
    {
        Vector3 randomDirection = enemy.transform.position + Random.insideUnitSphere * enemy.PatrolRadius;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, enemy.PatrolRadius, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }
    }

    private void StartWait()
    {
        isWaiting = true;
        waitTimer = (int)Random.Range(enemy.MinWaitTimer, enemy.MaxWaitTimer + 1f);
        enemy.Agent.ResetPath();
        //Debug.Log($"waitTimer enemy - {enemy.name} je: {waitTimer}");
    }
}
