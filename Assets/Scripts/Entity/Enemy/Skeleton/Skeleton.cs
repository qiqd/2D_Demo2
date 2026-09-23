using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Skeleton : Enemy
{
    public override void Awake()
    {
        base.Awake();
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
        DetectGroundAndWall(false);
    }
}
