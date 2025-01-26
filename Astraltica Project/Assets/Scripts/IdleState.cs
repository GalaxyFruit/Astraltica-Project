using UnityEngine;

public class IdleState : IEnemyState
{
    private EnemyAI enemy;
    private float idleTimer;
    private float maxIdleTime;

    public IdleState(EnemyAI enemyAI)
    {
        enemy = enemyAI;
        maxIdleTime = Random.Range(5f, 10f);
    }

    public void EnterState()
    {
        enemy.Agent.ResetPath();
        enemy.AnimationController.SetMoveSpeed(0);
        idleTimer = 0;
    }

    public void UpdateState()
    {
        idleTimer += enemy.StateUpdateInterval;

        if (enemy.IsPlayerInRange(enemy.DetectionRange))
        {
            enemy.SwitchState(new ChaseState(enemy));
            return;
        }

        if (idleTimer >= maxIdleTime)
        {
            enemy.SwitchState(new PatrolState(enemy));
            return;
        }

        enemy.UpdateMovementAnimation();
    }

    public void ExitState()
    {
        // Nic speciálního při opuštění stavu Idle
    }
}
