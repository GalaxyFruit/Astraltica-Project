using UnityEngine;
using UnityEngine.AI;

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
            if (enemy.Agent.pathStatus == NavMeshPathStatus.PathPartial || enemy.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Debug.Log("[ChaseState] Cesta k hráči není platná. Přepínám do IdleState.");
                enemy.SwitchState(new IdleState(enemy));
            }
        }

        enemy.UpdateMovementAnimation();
    }

    public void ExitState()
    {
        enemy.Agent.ResetPath();
    }
}
