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
        enemy.AnimationController.ApplyDamageAfterCurrentAttack();
        Debug.Log($"[AttackState] Začínám útok. Timer nastaven na: {attackTimer}");
    }

    public void UpdateState()
    {
        attackTimer -= enemy.StateUpdateInterval;

        if (!enemy.IsPlayerInRange(enemy.AttackRange))
        {
            enemy.SwitchState(new ChaseState(enemy));
            return;
        }

        if (attackTimer <= 0f)
        {
            enemy.AnimationController.PlayAttackAnimation();
            attackTimer = enemy.AnimationController.GetCurrentAttackDuration();
            enemy.AnimationController.ApplyDamageAfterCurrentAttack();
        }
    }

    public void ExitState()
    {
        enemy.AnimationController.StopAttackAnimation();
        Debug.Log("[AttackState] Ukončuji stav.");
    }
}
