using System;
using UnityEngine;

class PlayerAirState : EntityState
{
    public Player player;
    public PlayerAirState(Player player, StateMachine stateMachine, string condition) : base(player.animator, stateMachine, condition)
    {
        this.player = player;
    }

    public override void Update()
    {
        player.animator.SetFloat("yInput", Mathf.Clamp(player.rigidbody2D.velocity.y, -1f, 1f));
        if (player.isGrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.moveDirection.x != 0)
        {
            player.rigidbody2D.velocity = new Vector2(player.moveDirection.x * player.moveSpeed, player.rigidbody2D.velocity.y);
        }
        if (player.isWallDetected)
        {
            stateMachine.ChangeState(player.slideState);
        }
        if (player.inputActions.keyboard.Dash.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.dashState);
        }
        if (player.inputActions.keyboard.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);

        }
    }
}