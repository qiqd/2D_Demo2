using System.Collections;
using UnityEngine;

/// <summary>
/// 玩家控制器类，挂载在玩家角色上，负责驱动状态机的初始化和每帧更新。
/// 通过状态机模式管理玩家的各种行为状态（如 idle、move 等）。
/// 同时处理新输入系统（PlayerInput）的初始化和输入事件绑定。
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>状态机实例，外部只读，内部在 Awake 中创建</summary>
    public StateMachine stateMachine { get; private set; }
    /// <summary>玩家的待机状态实例</summary>
    public EntityState idleState;
    /// <summary>玩家的移动状态实例</summary>
    public EntityState runState;

    public EntityState attackState;

    public EntityState airState;
    public EntityState slideState;
    public EntityState wallJumpState;
    public EntityState dashState;
    public EntityState jumpAttackState;
    public PlayerInput inputActions;
    public Vector2 moveDirection;
    public Animator animator;
    public Rigidbody2D rigidbody2D;
    private CapsuleCollider2D capsuleCollider;
    [SerializeField]
    public float moveSpeed = 4f;
    [SerializeField]
    public float jumpSpeed = 10f;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    public bool isGrounded = true;
    [SerializeField]
    public bool isWallDetected = false;
    [SerializeField]
    [Min(0)]
    private float groundCheckDistance = 0.15f;
    [SerializeField]
    [Min(0)]
    private float slideCheckDistance = 0.15f;
    [Min(0)]
    public float dashSpeed = 10f;
    [Min(0.1f)]
    public float dashTime = 0.25f;
    public bool facingRight = true;
    /// <summary>攻击动画剪辑引用，用于计算攻击状态的超时兜底时长</summary>
    public AnimationClip attackClip;

    public Coroutine attackCoroutine;
    /// <summary>
    /// Unity 生命周期函数：在脚本加载时调用（早于 Start）。
    /// 用于创建输入系统、状态机实例和各状态对象。
    /// </summary>
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        inputActions = new PlayerInput();
        stateMachine = new StateMachine();
        // 每个状态使用各自专属的动画参数，避免多个状态共用同一 bool 互相干扰
        idleState = new PlayerIdleState(this, stateMachine, "isIdle");
        runState = new PlayerRunState(this, stateMachine, "isMove");
        airState = new PlayerAirState(this, stateMachine, "isAired");   // 空中 = isGrounded false
        attackState = new PlayerAttackState(this, stateMachine, "isAttacking");
        slideState = new PlayerWallSlideState(this, stateMachine, "isSliding");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "wallSliderJump");
        dashState = new PlayerDashState(this, stateMachine, "isDashing");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "jumpAttack");
        moveDirection = Vector2.zero;
    }

    /// <summary>
    /// Unity 生命周期函数：当对象启用时调用。
    /// 启用输入系统，并绑定 Movement 输入事件的回调：
    /// - performed：按键按下/持续时，读取二维向量值赋值给 moveDirection
    /// - canceled：按键松开时，将 moveDirection 重置为零向量
    /// </summary>
    void OnEnable()
    {
        inputActions.Enable();
        inputActions.keyboard.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        inputActions.keyboard.Movement.canceled += ctx => moveDirection = Vector2.zero;
        inputActions.keyboard.Jump.performed += cts => setJump();
    }

    /// <summary>
    /// Unity 生命周期函数：当对象禁用时调用。
    /// 禁用输入系统，停止接收输入事件。
    /// </summary>
    void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// Unity 生命周期函数：在第一次帧更新前调用。
    /// 初始化状态机，将待机状态设为初始状态并触发其 Enter 逻辑。
    /// </summary>
    void Start()
    {
        stateMachine.Initialize(idleState);
    }

    /// <summary>
    /// Unity 生命周期函数：每帧调用一次。
    /// 驱动当前状态的 Update 逻辑，由状态内部处理移动、动画、状态切换判断等。
    /// </summary>
    void Update()
    {
        FlipPlayer();
        DetectGroundAndWall();
        stateMachine.currentState.Update();
    }



    void OnDrawGizmos()
    {
        DrawGizmos();
    }


    void FlipPlayer()
    {
        if (moveDirection.x > 0f && !facingRight)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveDirection.x < 0f && facingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        facingRight = !facingRight;

    }

    /// <summary>
    /// 设定水平方向移动速度（保留现有竖直速度，例如跳跃/下落）。
    /// 此方法供各个状态调用，保证待机之外（含空中）都能响应水平输入。
    /// </summary>
    public void setMovement()
    {
        rigidbody2D.velocity = new Vector2(moveDirection.x * moveSpeed, rigidbody2D.velocity.y);
    }

    /// <summary>
    /// 跳跃：仅在地面时给一个向上的竖直初速度。
    /// 起跳后由状态机根据 isGrounded / 速度切到 upState。
    /// </summary>
    public void setJump()
    {
        if (isGrounded) rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);

    }

    /// <summary>
    /// 绘制辅助线和碰撞体的 Gizmos，用于调试接地检测和贴墙检测。
    /// </summary>
    void DrawGizmos()
    {
        // 绘制一条从脚底点向下延伸的辅助线，便于观察接地检测的射线范围
        Vector2 foot = GetFootPoint();
        Vector2 front = GetFrontPoint();
        Gizmos.DrawLine(foot, foot + Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(front, front + Vector2.right * slideCheckDistance);
    }

    /// <summary>
    /// 计算玩家脚底（胶囊碰撞体底部中心）的世界坐标。
    /// 用于让接地射线从脚底出发、距离更短更准确，避免从模型中心向下扫描导致的误判。
    /// </summary>
    Vector2 GetFootPoint()
    {
        if (capsuleCollider == null) return (Vector2)transform.position;
        // 胶囊体底部 = 中心 + offset 再减去高度的一半
        Vector2 center = (Vector2)transform.position + capsuleCollider.offset;
        float radius = capsuleCollider.size.x * 0.5f;
        float half = capsuleCollider.size.y * 0.5f - radius;  // 竖直方向上下延伸
        return center - new Vector2(0f, half + radius + 0.02f); // 放在碰撞体下表面略下方
    }
    /// <summary>
    /// 计算玩家前方（胶囊碰撞体侧面中心）的世界坐标。
    /// </summary>
    /// <returns></returns>
    Vector2 GetFrontPoint()
    {
        if (capsuleCollider == null) return (Vector2)transform.position;
        Vector2 center = (Vector2)transform.position + capsuleCollider.offset;
        float radius = capsuleCollider.size.x * 0.5f;
        return center + new Vector2(moveDirection.x * radius + 0.02f, 0f);
    }

    /// <summary>
    /// 检测玩家是否接地或贴墙，并更新 isGrounded 和 isWallDetected 状态。
    /// </summary>
    void DetectGroundAndWall()
    {
        bool hit = Physics2D.Raycast(GetFootPoint(), Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = hit;
        isWallDetected = Physics2D.Raycast(GetFrontPoint(), Vector2.right * (facingRight ? 1 : -1), slideCheckDistance, whatIsGround);
        animator.SetBool("isSliding", isWallDetected);
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