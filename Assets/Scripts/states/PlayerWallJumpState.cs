public class PlayerWallJumpState : EntityState
{
    public Player player;
    public PlayerWallJumpState(Player player, StateMachine stateMachine, string condition) : base(player.animator, stateMachine, condition)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        player.inputActions.keyboard.Movement.Disable();
        player.rigidbody2D.velocity = new UnityEngine.Vector2(player.jumpSpeed * (player.facingRight ? -1 : 1), player.jumpSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        player.inputActions.keyboard.Movement.Enable();
    }

    public override void Update()
    {
        player.stateMachine.ChangeState(player.airState);
    }
}