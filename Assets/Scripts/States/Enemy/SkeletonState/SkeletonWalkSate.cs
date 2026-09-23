using Unity.Burst.Intrinsics;
using UnityEngine;

public class SkeletonWalkState : EntityState
{
    private Skeleton skeleton;
    public SkeletonWalkState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }

    public override void Update()
    {
        if (!skeleton.isGrounded || skeleton.isWallDetected)
        {
            skeleton.FlipEntity();
        }
        skeleton.rigidbody2D.velocity = new Vector2(skeleton.moveSpeed * (skeleton.facingRight ? 1 : -1), skeleton.rigidbody2D.velocity.y);
    }
}