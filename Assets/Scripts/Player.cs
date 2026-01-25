using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;

    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite defaultSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    private bool jumpRequested;
    private bool isGrounded;
    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;

    private int jumpsRemaining;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        jumpsRemaining = maxJumps;
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

        if ((moveX < 0f && isTouchingWallLeft) ||
        (moveX > 0f && isTouchingWallRight))
        {
            moveX = 0f;
        }
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        FlipSprite(moveX);

        animator.SetBool("IsMoving", Mathf.Abs(moveX) > 0.01f);
    }

    private void HandleJump()
    {
        if (jumpRequested && jumpsRemaining > 0)
        {
            if (isTouchingWallLeft || isTouchingWallRight)
            {
                jumpRequested = false;
                return;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
            audioSource.pitch = jumpsRemaining > 0 ? 1f : 1.2f;
            audioSource.Play();
        }
        jumpRequested = false;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsGrounded", isGrounded);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpsRemaining = maxJumps;
                break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isTouchingWallLeft = false;
        isTouchingWallRight = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x > 0.5f)
            {
                isTouchingWallLeft = true;
            }
            if (contact.normal.x < -0.5f)
            {
                isTouchingWallRight = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        isTouchingWallLeft = false;
        isTouchingWallRight = false;
    }

}