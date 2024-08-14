using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float _timer;
    //private float _recoveryTime = 2f;
    private int _chosenAttack;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        //_chosenAttack = enemy.ChooseRandomCombo();
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

        if (!enemy.IsWithinStrikingDistance)
        {
            _timer += Time.deltaTime;

            /*if (_timer > _recoveryTime && enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ready"))
            {
                enemy.Animator.SetBool($"Combo {chosenAttack}", false);
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }*/
            enemy.Animator.SetBool("IsWithinStrikingDistance", false);
            if (enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !enemy.Animator.GetBool("IsAttacking"))
            {
                enemy.SetDoingComboStatus(false);
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else 
        {
            /*if (enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ready"))
            {
                if (enemy.Animator.GetBool($"Combo {_chosenAttack}"))
                {
                    enemy.Animator.SetBool($"Combo {_chosenAttack}", false);
                    _chosenAttack = enemy.ChooseRandomCombo();
                    enemy.Animator.SetBool($"Combo {_chosenAttack}", true);
                }
                else
                {
                    _chosenAttack = enemy.ChooseRandomCombo();
                    enemy.Animator.SetBool($"Combo {_chosenAttack}", true);
                }
            }
            _timer = 0;*/

            enemy.Animator.SetBool("IsWithinStrikingDistance", true);
            if (enemy.Animator.GetBool("IsAttacking"))
            {
                enemy.SetDoingComboStatus(true);
            }
            if (!enemy.IsFacingPlayer() && !enemy.Animator.GetBool("IsAttacking"))
            {
                enemy.RotateTowardsPlayer();
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
