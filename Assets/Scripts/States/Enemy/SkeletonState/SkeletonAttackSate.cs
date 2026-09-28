using UnityEngine;

public class SkeletonAttackState : EntityState
{
    private Skeleton skeleton;
    public SkeletonAttackState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.currentState.stateTriggerCalled = false;
    }

    public override void Update()
    {
        if (stateMachine.currentState.stateTriggerCalled)
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}