using UnityEngine;

/// <summary>
/// 玩家的待机状态，继承自 EntityState 状态基类。
/// 当玩家站在地面且没有移动输入时处于此状态。
/// 负责根据输入和是否离地切换到移动 / 上升 / 下落状态。
/// </summary>
class PlayerIdleState : EntityState
{

    public PlayerIdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.rigidbody2D.velocity = Vector2.zero;
    }
    public override void Update()
    {
        // 1. 有左右输入 → 地面移动
        if (player.moveDirection.x != 0f)
        {
            stateMachine.ChangeState(player.runState);
        }
        if (!player.isGrounded && player.rigidbody2D.velocity.y != 0f)
        {
            stateMachine.ChangeState(player.airState);
        }
        if (player.inputActions.keyboard.Dash.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.dashState);
        }
        if (player.inputActions.keyboard.Attack.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.attackState);
        }
    }
}