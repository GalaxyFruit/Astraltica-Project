using UnityEngine;

public class IdleState : IEnemyState
{
    private EnemyAI enemy;

    public IdleState(EnemyAI enemyAI)
    {
        enemy = enemyAI;
    }

    public void EnterState()
    {
        enemy.Agent.ResetPath();
        enemy.AnimationController.SetMoveSpeed(0);
    }

    public void UpdateState()
    {
        if (enemy.IsPlayerInRange(enemy.DetectionRange))
        {
            enemy.SwitchState(new ChaseState(enemy));
        }
    }

    public void ExitState()
    {
        // Nic speciálního při opuštění stavu Idle
    }
}
