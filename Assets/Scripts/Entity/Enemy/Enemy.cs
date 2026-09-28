using System;
using UnityEngine;

public class Enemy : Entity
{
    public EntityState idleState;
    public EntityState walkState;
    public EntityState attackState;
    public EntityState deathState;
    public EntityState hitState;

    public override void FlipEntity()
    {
        facingRight = !facingRight;
        var currentScale = transform.localScale;
        transform.localScale = new Vector3(facingRight ? Math.Abs(currentScale.x) : -Math.Abs(currentScale.x), currentScale.y, currentScale.z);
    }


}