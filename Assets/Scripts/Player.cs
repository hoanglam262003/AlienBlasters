using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        JumpAnimation();
    }

    private void JumpAnimation()
    {
        float moveX = GameInput.Instance.GetMoveHorizontal();
        FlipSprite(moveX);

        spriteRenderer.sprite = IsGrounded() ? defaultSprite : jumpSprite;
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        jumpRequested = false;
    }

    private bool IsGrounded()
    {
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
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
