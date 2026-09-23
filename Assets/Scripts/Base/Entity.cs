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
        stateMachine.currentState.Update();
    }

    public virtual void FlipEntity()
    {
        if (moveDirection.x > 0f && !facingRight)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            facingRight = !facingRight;
        }
        else if (moveDirection.x < 0f && facingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            facingRight = !facingRight;
        }

    }

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
        Vector2 centerV = (Vector2)transform.position + capsuleCollider.offset;
        float radius = capsuleCollider.size.x * 0.5f;
        float height = capsuleCollider.size.y * 0.5f;
        if (center)
        {
            return centerV + new Vector2(0f, -height - 0.02f);
        }
        else
        {
            return centerV + new Vector2(facingRight ? radius : -radius, -height - 0.02f);
        }
    }

    public Vector2 GetFrontPoint()
    {
        if (capsuleCollider == null) return (Vector2)transform.position;
        Vector2 center = (Vector2)transform.position + capsuleCollider.offset;
        float radius = capsuleCollider.size.x * 0.5f;
        return center + new Vector2(moveDirection.x * radius + 0.02f, 0f);
    }




    public virtual void DetectGroundAndWall(bool center)
    {
        isGrounded = Physics2D.Raycast(GetFootPoint(center), Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(GetFrontPoint(), Vector2.right * (facingRight ? 1 : -1), slideCheckDistance, whatIsGround);
    }

}