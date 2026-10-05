using System;
using UnityEngine;

public class Enemy : Entity
{
    public SkeletonIdleState idleState;
    public SkeletonWalkState walkState;
    public SkeletonAttackState attackState;
    // public SkeletonDeathState deathState;
    // public SkeletonHitState hitState;
    public Transform player;
    public LayerMask playerLayer;
    public float playerCheckDistance = 2;
    public bool isPlayerDetected = false;
    public Vector3 vectorDistance;

    public override void FlipEntity()
    {

        facingRight = !facingRight;
        var currentScale = transform.localScale;
        transform.localScale = new Vector3(facingRight ? Math.Abs(currentScale.x) : -Math.Abs(currentScale.x), currentScale.y, currentScale.z);
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(player.transform.position, new Vector3(player.transform.position.x + (facingRight ? playerCheckDistance : -playerCheckDistance), player.transform.position.y, player.transform.position.z));

    }

    public override void Update()
    {
        PlayerDetection2();
    }

    /// <summary>
    /// 只在朝向与目标方向不一致时才翻转，避免每帧无脑翻转导致抖动/原地打转
    /// </summary>
    public void FlipTowards(float targetX)
    {
        bool shouldFaceRight = targetX >= transform.position.x;
        if (shouldFaceRight != facingRight)
        {
            FlipEntity();
        }
    }

    public RaycastHit2D PlayerDetection()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.right * (facingRight ? 1 : -1), playerCheckDistance, playerLayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        return hit;
    }

    public virtual void PlayerDetection2()
    {
        var distance = Vector2.Distance(transform.position, player.transform.position);
        vectorDistance = player.transform.position - transform.position;
        isPlayerDetected = distance <= playerCheckDistance;

    }

}