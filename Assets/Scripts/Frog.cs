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
    [SerializeField]
    private int jumps = 2;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int jumpRemaining;

    protected override void Awake()
    {
        maxHP = 3;
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        defaultSprite = spriteRenderer.sprite;
        jumpRemaining = jumps;

        InvokeRepeating(nameof(Jump), jumpDelay, jumpDelay);
    }

    private void Jump()
    {
        if (jumpRemaining <= 0)
        {
            jumpForce *= new Vector2(-1, 1);
            jumpRemaining = jumps;
        }
        jumpRemaining--;
        rb.AddForce(jumpForce);
        spriteRenderer.flipX = jumpForce.x > 0;
        spriteRenderer.sprite = jumpSprite;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        spriteRenderer.sprite = defaultSprite;
        if (audioSource != null && audioSource.enabled && audioSource.gameObject.activeInHierarchy)
        {
            audioSource.Play();
        }
        base.OnCollisionEnter2D(collision);
    }
}
