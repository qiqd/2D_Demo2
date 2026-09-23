

using UnityEngine;

class PlayerWallSlideState : EntityState
{
    public Player player;
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string condition) : base(player.animator, stateMachine, condition)
    {
        this.player = player;
    }
    public override void Enter()
    {
        base.Enter();
        player.rigidbody2D.velocity = Vector2.zero;
    }

    public override void Update()
    {
        if (player.inputActions.keyboard.Jump.WasPerformedThisFrame() && player.isWallDetected)
            stateMachine.ChangeState(player.wallJumpState);

        if (!player.isWallDetected)
            stateMachine.ChangeState(player.airState);

        if (player.moveDirection.y > 0)
            player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, player.rigidbody2D.velocity.y * 0.9f);

        if (player.isGrounded)
            stateMachine.ChangeState(player.idleState);

    }
}