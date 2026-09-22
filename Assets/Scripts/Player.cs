using System.Collections;
using UnityEngine;


/// <summary>
/// 玩家类，继承自实体类，包含玩家的各种状态和输入处理
/// </summary>
public class Player : Entity
{
    public EntityState idleState;
    public EntityState runState;
    public EntityState attackState;
    public EntityState airState;
    public EntityState slideState;
    public EntityState wallJumpState;
    public EntityState dashState;
    public EntityState jumpAttackState;
    public PlayerInput inputActions;
    [SerializeField]
    public float moveSpeed = 4f;
    [SerializeField]
    public float jumpSpeed = 10f;
    [SerializeField]
    [Min(0)]
    public float dashSpeed = 10f;
    [Min(0.1f)]
    public float dashTime = 0.25f;
    public AnimationClip attackClip;
    public Coroutine attackCoroutine;
    public override void Awake()
    {
        base.Awake();
        inputActions = new PlayerInput();
        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "isIdle");
        runState = new PlayerRunState(this, stateMachine, "isMove");
        airState = new PlayerAirState(this, stateMachine, "isAired");
        attackState = new PlayerAttackState(this, stateMachine, "isAttacking");
        slideState = new PlayerWallSlideState(this, stateMachine, "isSliding");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "wallSliderJump");
        dashState = new PlayerDashState(this, stateMachine, "isDashing");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "jumpAttack");
        moveDirection = Vector2.zero;
    }
    public override void Start()
    {
        base.Awake();
        stateMachine.Initialize(idleState);
    }

    public override void Update()
    {
        base.Update();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.keyboard.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        inputActions.keyboard.Movement.canceled += ctx => moveDirection = Vector2.zero;
        inputActions.keyboard.Jump.performed += cts => setJump();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void OnDrawGizmos()
    {
        DrawGizmos();
    }

    public void setMovement()
    {
        rigidbody2D.velocity = new Vector2(moveDirection.x * moveSpeed, rigidbody2D.velocity.y);
    }

    public void setJump()
    {
        if (isGrounded) rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);

    }

    public IEnumerator EnterAttackStateWithCor()
    {
        yield return new WaitForEndOfFrame();
        if (stateMachine.currentState == attackState)
        {
            stateMachine.ChangeState(attackState);
        }

    }

    public void EnterAttackStateWithDelay()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(EnterAttackStateWithCor());
    }
}