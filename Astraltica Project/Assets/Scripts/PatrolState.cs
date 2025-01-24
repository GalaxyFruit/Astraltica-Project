using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IEnemyState
{
    private EnemyAI enemy;

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
        else if (!enemy.Agent.hasPath)
        {
            MoveToRandomPoint();
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
}
