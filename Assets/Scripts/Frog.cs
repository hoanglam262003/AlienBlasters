using UnityEngine;

public class Frog : Enemy
{
    [SerializeField]
    private float jumpDelay = 2f;
    [SerializeField]
    private Vector2 jumpForce;
    [SerializeField]
    private Sprite jumpSprite;
    [SerializeField]
    private Sprite defaultSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;

        InvokeRepeating("Jump", jumpDelay, jumpDelay);
    }

    private void Jump()
    {
        rb.AddForce(jumpForce);
        jumpForce *= new Vector2(-1, 1);
        FlipSprite();
        spriteRenderer.sprite = jumpSprite;
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        spriteRenderer.sprite = defaultSprite;
        base.OnCollisionEnter2D(collision);
    }
}
