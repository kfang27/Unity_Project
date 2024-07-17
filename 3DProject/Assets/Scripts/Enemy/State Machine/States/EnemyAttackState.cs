using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float _timer;
    private float _recoveryTime = 2f;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        enemy.Animator.SetBool("Attack", enemy.IsWithinStrikingDistance);
        Debug.Log("ATTACK");
    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.Animator.SetBool("Attack", enemy.IsWithinStrikingDistance);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!enemy.IsFacingPlayer())
        {
            enemy.RotateTowardsPlayer();
        }

        int chosenAttack = -1;

        if (!enemy.IsWithinStrikingDistance)
        {
            _timer += Time.deltaTime;

            if (_timer > _recoveryTime && enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ready"))
            {
                if (chosenAttack != -1)
                {
                    enemy.Animator.SetBool($"Combo {chosenAttack}", false);
                }
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else 
        {
            if (enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ready"))
            {
                if (chosenAttack != -1 && enemy.Animator.GetBool($"Combo {chosenAttack}"))
                {
                    enemy.Animator.SetBool($"Combo {chosenAttack}", false);
                    chosenAttack = enemy.ChooseRandomCombo();
                    enemy.Animator.SetBool($"Combo {chosenAttack}", true);
                }
                else
                {
                    chosenAttack = enemy.ChooseRandomCombo();
                    enemy.Animator.SetBool($"Combo {chosenAttack}", true);
                }
            }
            _timer = 0;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
