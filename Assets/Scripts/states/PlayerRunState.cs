using UnityEngine;

/// <summary>
/// 玩家的地面移动状态，继承自 EntityState 状态基类。
/// 当玩家站在地面且有左右输入时处于此状态。
/// 负责执行地面水平移动，并根据输入/是否离地切换到待机或上升/下落。
/// </summary>
class PlayerRunState : EntityState
{
    public Player player;
    public PlayerRunState(Player player, StateMachine stateMachine, string stateName) : base(player.animator, stateMachine, stateName)
    {
        this.player = player;
    }

    public override void Update()
    {
        if (!player.isGrounded && player.rigidbody2D.velocity.y != 0f)
        {
            stateMachine.ChangeState(player.airState);

        }
        if (player.rigidbody2D.velocity.x == 0f)
        {
            stateMachine.ChangeState(player.idleState);

        }
        if (player.inputActions.keyboard.Dash.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.dashState);
            return;
        }
        if (player.inputActions.keyboard.Attack.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.attackState);
            return;
        }
        player.setMovement();

    }
}