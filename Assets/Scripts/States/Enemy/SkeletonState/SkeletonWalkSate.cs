using Unity.Burst.Intrinsics;
using UnityEngine;

public class SkeletonWalkState : EnemyState
{
    private Skeleton skeleton;
    public SkeletonWalkState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }

    public override void Update()
    {
        // 检测到玩家：先动态调整朝向面朝玩家，再进入攻击
        if (skeleton.isPlayerDetected && Mathf.Abs(skeleton.vectorDistance.y) < 1.5f)
        {
            skeleton.FlipTowards(skeleton.player.position.x);
            stateMachine.ChangeState(skeleton.attackState);
            return;
        }

        // 未检测到玩家：沿用边缘/墙巡逻翻转
        if (!skeleton.isGrounded || skeleton.isWallDetected)
        {
            skeleton.FlipEntity();
        }

        skeleton.rigidbody2D.velocity = new Vector2(skeleton.moveSpeed * (skeleton.facingRight ? 1 : -1), skeleton.rigidbody2D.velocity.y);
    }
}