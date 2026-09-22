using UnityEngine;

public class PlayerDashState : EntityState
{
    private float timer = 0;
    public PlayerDashState(Player player, StateMachine stateMachine, string condition) : base(player, stateMachine, condition)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // player.inputActions.keyboard.Movement.Disable();
        player.rigidbody2D.gravityScale = 0;
        Rigidbody2D rigidbody2D = player.rigidbody2D;
        float dashDirection = player.facingRight ? 1 : -1;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.velocity = new Vector2(dashDirection * player.dashSpeed, rigidbody2D.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();
        player.rigidbody2D.gravityScale = 1;
        //   player.inputActions.keyboard.Movement.Enable();
        timer = 0;
    }

    public override void Update()
    {
        Debug.Log("Timer:" + timer);
        if (timer >= player.dashTime)
        {
            if (player.isGrounded)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.airState);
            }
        }

        if (player.isWallDetected)
        {
            stateMachine.ChangeState(player.slideState);
            return;
        }
        timer += Time.deltaTime;
    }
}