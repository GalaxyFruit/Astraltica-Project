using UnityEngine;

public class ChaseState : IEnemyState
{
    private EnemyAI enemy;

    public ChaseState(EnemyAI enemyAI)
    {
        enemy = enemyAI;
    }

    public void EnterState()
    {
        enemy.Agent.speed = enemy.ChaseSpeed;
    }

    public void UpdateState()
    {
        if (enemy.IsPlayerInRange(enemy.AttackRange))
        {
            enemy.SwitchState(new AttackState(enemy));
        }
        else if (!enemy.IsPlayerInRange(enemy.DetectionRange))
        {
            enemy.SwitchState(new PatrolState(enemy));
        }
        else
        {
            enemy.Agent.SetDestination(enemy.Player.position);
        }

        enemy.UpdateMovementAnimation();
    }

    public void ExitState()
    {
        enemy.Agent.ResetPath();
    }
}
