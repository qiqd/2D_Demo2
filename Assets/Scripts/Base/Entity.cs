using UnityEngine;
/// <summary>
/// 实体类，所有实体类都继承自该类
/// </summary>
public abstract class Entity : MonoBehaviour
{
    public Animator animator;
    public StateMachine stateMachine;
    public new Rigidbody2D rigidbody2D;
    public CapsuleCollider2D capsuleCollider;
    public Vector2 moveDirection;
    public LayerMask whatIsGround;
    public bool isGrounded = true;
    public bool isWallDetected = false;
    [SerializeField]
    public float moveSpeed = 4f;
    [SerializeField]
    public float jumpSpeed = 10f;
    [SerializeField]
    [Min(0)]
    public float dashSpeed = 10f;
    [Min(0)]
    private float groundCheckDistance = 0.15f;
    [Min(0)]
    private float slideCheckDistance = 0.15f;
    public bool facingRight = true;

    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        FlipEntity();
        // 状态机在 Awake 中创建；Initialize 之前 currentState 可能为 null（例如 Awake 被跳过或初始化顺序异常）
        if (stateMachine != null && stateMachine.currentState != null)
            stateMachine.currentState.Update();
    }

    public abstract void FlipEntity();



    public virtual void DrawGizmos()
    {

    }
    /// <summary>
    ///获取实体底部的点,当center为true时,返回胶囊体底部的中心点,否则根据facingRight,返回胶囊体的左侧或右侧点
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    public Vector2 GetFootPoint(bool center)
    {
        if (capsuleCollider == null) return (Vector2)transform.position;

        // 使用世界空间的 bounds, 自动包含 transform 的缩放(如 Skeleton 的 1.22)
        Bounds b = capsuleCollider.bounds;
        float radius = b.extents.x;
        if (center)
        {
            return new Vector2(b.center.x, b.min.y - 0.02f);
        }
        else
        {
            return new Vector2(b.center.x + (facingRight ? radius : -radius), b.min.y - 0.02f);
        }
    }

    public Vector2 GetFrontPoint()
    {
        if (capsuleCollider == null) return (Vector2)transform.position;

        // 使用世界空间的 bounds, 自动包含 transform 的缩放
        Bounds b = capsuleCollider.bounds;
        float radius = b.extents.x;
        return new Vector2(b.center.x + (facingRight ? radius + 0.02f : -radius - 0.02f), b.center.y);
    }




    public virtual void DetectGroundAndWall(bool center)
    {
        isGrounded = Physics2D.Raycast(GetFootPoint(center), Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(GetFrontPoint(), Vector2.right * (facingRight ? 1 : -1), slideCheckDistance, whatIsGround);
    }

}