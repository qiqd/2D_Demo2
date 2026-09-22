using UnityEngine;
/// <summary>
/// 实体类，所有实体类都继承自该类
/// </summary>
public abstract class Entity : MonoBehaviour
{
    public Animator animator;
    public StateMachine stateMachine;
    public Rigidbody2D rigidbody2D;
    public CapsuleCollider2D capsuleCollider;
    public Vector2 moveDirection;
    public LayerMask whatIsGround;
    public bool isGrounded = true;
    public bool isWallDetected = false;
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
        FlipPlayer();
        DetectGroundAndWall();
        stateMachine.currentState.Update();
    }

    public virtual void FlipPlayer()
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

    public Vector2 GetFootPoint()
    {
        if (capsuleCollider == null) return (Vector2)transform.position;
        // 胶囊体底部 = 中心 + offset 再减去高度的一半
        Vector2 center = (Vector2)transform.position + capsuleCollider.offset;
        float radius = capsuleCollider.size.x * 0.5f;
        float half = capsuleCollider.size.y * 0.5f - radius;  // 竖直方向上下延伸
        return center - new Vector2(0f, half + radius + 0.02f); // 放在碰撞体下表面略下方
    }

    public Vector2 GetFrontPoint()
    {
        if (capsuleCollider == null) return (Vector2)transform.position;
        Vector2 center = (Vector2)transform.position + capsuleCollider.offset;
        float radius = capsuleCollider.size.x * 0.5f;
        return center + new Vector2(moveDirection.x * radius + 0.02f, 0f);
    }




    public void DetectGroundAndWall()
    {
        bool hit = Physics2D.Raycast(GetFootPoint(), Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = hit;
        isWallDetected = Physics2D.Raycast(GetFrontPoint(), Vector2.right * (facingRight ? 1 : -1), slideCheckDistance, whatIsGround);
        animator.SetBool("isSliding", isWallDetected);
    }


}