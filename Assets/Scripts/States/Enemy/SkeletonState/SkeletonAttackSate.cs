using System;
using Unity.Mathematics;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Skeleton skeleton;
    public SkeletonAttackState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }
    public override void Enter()
    {
        base.Enter();
        if (Math.Abs(skeleton.vectorDistance.x) < 1)
        {
            skeleton.rigidbody2D.velocity = new Vector2(skeleton.facingRight ? 1 : -1, skeleton.rigidbody2D.velocity.y);
        }
        else
        {
            skeleton.rigidbody2D.velocity = Vector2.zero;

        }

        stateMachine.currentState.stateTriggerCalled = false;
    }

    public override void Update()
    {
        // 攻击期间持续面向玩家，避免背身攻击
        skeleton.FlipTowards(skeleton.player.position.x);


        if (stateMachine.currentState.stateTriggerCalled)
        {
            stateMachine.ChangeState(skeleton.walkState);
        }

    }
}