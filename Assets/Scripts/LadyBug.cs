using UnityEngine;

public class LadyBug : Enemy
{
    [SerializeField]
    private float speed = 1f;
    private Vector2 direction = Vector2.left;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Rigidbody2D rb;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }
}
