using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyAI enemy;
    private float attackTimer;

    public AttackState(EnemyAI enemyAI)
    {
        enemy = enemyAI;
    }

    public void EnterState()
    {
        enemy.Agent.ResetPath();
        attackTimer = enemy.AnimationController.GetCurrentAttackDuration();
        enemy.AnimationController.PlayAttackAnimation();
        Debug.Log($"[AttackState] Začínám útok. Timer nastaven na: {attackTimer}");
    }

    public void UpdateState()
    {
        attackTimer -= enemy.StateUpdateInterval;

        if (attackTimer <= 0f)
        {
            Debug.Log("PODMÍNKAAAA");
            if (enemy.IsPlayerInRange(enemy.AttackRange))
            {
                enemy.AnimationController.PlayAttackAnimation();
                attackTimer = enemy.AnimationController.GetCurrentAttackDuration();
                Debug.Log($"attack timer je: {attackTimer}");
            }
            else
            {
                enemy.SwitchState(new ChaseState(enemy));
            }
        }
    }

    public void ExitState()
    {
        enemy.AnimationController.StopAttackAnimation();
        Debug.Log("[AttackState] Ukončuji stav.");
    }
}
