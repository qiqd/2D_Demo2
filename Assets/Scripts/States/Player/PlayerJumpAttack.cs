public class PlayerJumpAttackState : EntityState
{

    public Player player;
    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string condition) : base(player.animator, stateMachine, condition)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        stateTriggerCalled = false;
    }

    public override void Update()
    {
        if (stateTriggerCalled)
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
    }
}