using UnityEngine;

public class SkeletonIdleState : EntityState
{
    private Skeleton skeleton;
    public SkeletonIdleState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }

    public override void Update()
    {

    }
}