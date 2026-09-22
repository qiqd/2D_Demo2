using UnityEngine;

public class PlayerAttackState : EntityState
{

    public Player player;
    /// <summary>连击段位：0 = 第1段，1 = 第2段，2 = 第3段；达到 maxComboCount 视为需要重置</summary>
    private int attackIndex = 0;

    /// <summary>上一次攻击发生的时间点（跨攻击保留，用于判断连击是否中断）</summary>
    private float lastAttackTime = -999f;

    /// <summary>连击中断间隔：超过此秒数未再次攻击，则重置回第 1 段</summary>
    private const float comboResetInterval = 5f;

    /// <summary>最大连击段数</summary>
    private const int maxComboCount = 3;

    private bool comboAttack = false;
    public PlayerAttackState(Player player, StateMachine stateMachine, string condition) : base(player.animator, stateMachine, condition)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        comboAttack = false;
        stateTriggerCalled = false;
        player.rigidbody2D.velocity = Vector2.zero;
        // 1) 判断连击是否中断：距上次攻击超过 comboResetInterval，或已达段数上限，则重置回第 1 段
        bool tooLate = (Time.time - lastAttackTime) >= comboResetInterval;
        if (attackIndex >= maxComboCount || tooLate)
        {
            attackIndex = 0;
        }
        player.animator.SetInteger("attackIndex", attackIndex);
        lastAttackTime = Time.time;
        attackIndex += 1;
    }

    public override void Exit()
    {
        base.Exit();
        stateTriggerCalled = false;
    }

    public override void Update()
    {
        if (player.moveDirection.x != 0)
        {
            player.transform.localScale = player.moveDirection.x > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        }

        if (player.inputActions.keyboard.Attack.WasPressedThisFrame())
        {
            comboAttack = true;
        }
        if (stateTriggerCalled)
        {
            if (comboAttack)
            {
                base.Exit();
                player.EnterAttackStateWithDelay();
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
