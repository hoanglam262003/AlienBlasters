using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite defaultSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool jumpRequested;
    private bool IsGrounded => Mathf.Abs(rb.linearVelocity.y) < 0.01f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameInput.Instance.IsJumpPressed())
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        float moveX = GameInput.Instance.GetMoveHorizontal();

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        FlipSprite(moveX);

        // IsMoving cho Animator
        animator.SetBool("IsMoving", Mathf.Abs(moveX) > 0.01f);
    }

    private void HandleJump()
    {
        if (jumpRequested && IsGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        jumpRequested = false;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsGrounded", IsGrounded);
    }

    private void FlipSprite(float moveX)
    {
        if (moveX < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0f)
        {
            spriteRenderer.flipX = false;
        }
    }
}