using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skeleton : Enemy
{
    public override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine();
        idleState = new SkeletonIdleState(this, stateMachine, "isIdle");
        walkState = new SkeletonWalkState(this, stateMachine, "isWalk");
        attackState = new SkeletonAttackState(this, stateMachine, "isAttack");
    }

    public override void Start()
    {
        base.Start();

        stateMachine.Initialize(walkState);

    }

    public override void Update()
    {
        base.Update();
        if (stateMachine != null && stateMachine.currentState != null)
            stateMachine.currentState.Update();
        base.DetectGroundAndWall(false);
    }
}
