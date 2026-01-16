using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        float moveX = GameInput.Instance.GetMoveHorizontal();

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
}
