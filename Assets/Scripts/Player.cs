using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string IS_MOVING = "IsMoving";
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_ON_SNOW = "Snow";

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float snowSpeedMultiplier = 0.5f;

    [Header("Sprite")]
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite defaultSprite;

    [Header("Layers")]
    [SerializeField] private LayerMask resetJumpLayers;
    [SerializeField] private LayerMask groundLayers;

    [Header("Combat")]
    [SerializeField] private float invincibleTime = 1f;
    [SerializeField] private float knockbackForce = 8f;

    [Header("Audio")]
    [SerializeField] private AudioClip coinSfx;

    public int playerId { get; private set; }
    public int coinsCollected { get; private set; }
    public int health { get; private set; } = 3;
    public event Action<int> OnCoinsChanged;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    private bool jumpRequested;
    private bool isGrounded;
    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;
    private bool isOnSnow;
    private bool isInvincible;
    private bool isHurt;

    private int jumpsRemaining;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        jumpsRemaining = maxJumps;
    }

    private void Start()
    {
        PlayerRegistry.Instance.Register(this);
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
        if (!isHurt)
        {
            HandleMovement();
            HandleJump();
        }
        UpdateAnimator();
    }

    public void Init(int playerId)
    {
        this.playerId = playerId;
    }

    private void HandleMovement()
    {
        float moveX = GameInput.Instance.GetMoveHorizontal();

        if ((moveX < 0f && isTouchingWallLeft) ||
        (moveX > 0f && isTouchingWallRight))
        {
            moveX = 0f;
        }
        float currentSpeed = isOnSnow ? moveSpeed * snowSpeedMultiplier : moveSpeed;
        rb.linearVelocity = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
        FlipSprite(moveX);

        animator.SetBool(IS_MOVING, Mathf.Abs(moveX) > 0.01f);
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
        animator.SetBool(IS_GROUNDED, isGrounded);
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
        if (collision.gameObject.CompareTag(IS_ON_SNOW))
        {
            isOnSnow = true;
        }
        int otherLayer = collision.gameObject.layer;

        if (((1 << otherLayer) & groundLayers) == 0)
            return;

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
        if (collision.gameObject.CompareTag(IS_ON_SNOW))
        {
            isOnSnow = false;
        }
        int otherLayer = collision.gameObject.layer;

        if (((1 << otherLayer) & groundLayers) != 0)
        {
            isGrounded = false;
        }
        isTouchingWallLeft = false;
        isTouchingWallRight = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (((1 << otherLayer) & resetJumpLayers) != 0)
        {
            jumpsRemaining = maxJumps;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (((1 << otherLayer) & resetJumpLayers) == 0)
            return;

        if (rb.linearVelocity.y <= 0f)
        {
            jumpsRemaining = maxJumps;
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
        audioSource.PlayOneShot(coinSfx);
        OnCoinsChanged?.Invoke(coinsCollected);
    }

    public void GetHit(Vector2 hitDirection)
    {
        if (isInvincible)
            return;

        health--;

        if (health < 0)
        {
            SceneManager.LoadScene(0);
            return;
        }

        isInvincible = true;
        isHurt = true;

        rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDir = new Vector2(Mathf.Sign(hitDirection.x), 1f).normalized;

        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(HurtCoroutine());
    }

    private IEnumerator HurtCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        isHurt = false;

        StartCoroutine(InvincibleCoroutine());
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        float timer = 0f;
        while (timer < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}