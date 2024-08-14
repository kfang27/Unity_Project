using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        enemy.Animator.SetBool("Chase", enemy.IsAggroed);
        Debug.Log("CHASE");
    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.Animator.SetBool("Chase", enemy.IsAggroed);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        enemy.RotateTowardsPlayer();
        enemy.MoveTowardsPlayer();

        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
