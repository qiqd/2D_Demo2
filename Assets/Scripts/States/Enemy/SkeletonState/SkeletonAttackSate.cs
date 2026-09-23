using UnityEngine;

public class SkeletonAttackState : EntityState
{
    private Skeleton skeleton;
    public SkeletonAttackState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }

    public override void Update()
    {

    }
}