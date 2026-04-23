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
    private const string IS_DUCKING = "IsDucking";

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float snowSpeedMultiplier = 0.5f;
    [SerializeField] private float movingPlatformSpeedMultiplier = 2f;

    [Header("Sprite")]
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite defaultSprite;

    [Header("Layers")]
    [SerializeField] private LayerMask resetJumpLayers;
    [SerializeField] private LayerMask groundLayers;

    [Header("Combat")]
    [SerializeField] private Blaster blaster;
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private float knockbackForce = 8f;

    [Header("Audio")]
    [SerializeField] private AudioClip coinSfx;
    [SerializeField] private AudioClip hurtSfx;

    [Header("Physics")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Vector2 duckSize;
    [SerializeField] private Vector2 duckOffset;
    private Vector2 originalSize;
    private Vector2 originalOffset;

    public int playerId { get; private set; }
    public int coinsCollected { get; private set; }
    public int health { get; private set; } = 10;
    public event Action<int> OnCoinsChanged;
    public event Action<int> OnHealthChanged;

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
    private bool isOnMovingPlatform;
    private bool isDucking;

    private int jumpsRemaining;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
        jumpsRemaining = maxJumps;
        if (PlayerRegistry.Instance != null)
        {
            PlayerRegistry.Instance.Register(this);
        }
        else
        {
            Debug.LogError("PlayerRegistry not found!");
        }
    }

    private void Update()
    {
        if (GameInput.Instance.IsJumpPressed())
        {
            jumpRequested = true;
        }
        if (GameInput.Instance.IsShootPressed())
        {
            blaster.TryShoot();
        }
        isDucking = GameInput.Instance.IsDuckPressed() && isGrounded;
        if (isDucking)
        {
            boxCollider.size = duckSize;
            boxCollider.offset = duckOffset;
        }
        else
        {
            boxCollider.size = originalSize;
            boxCollider.offset = originalOffset;
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
        if (isDucking)
        {
            moveX = 0f;
        }
        if ((moveX < 0f && isTouchingWallLeft) ||
        (moveX > 0f && isTouchingWallRight))
        {
            moveX = 0f;
        }
        float currentSpeed = moveSpeed;

        if (isOnSnow)
        {
            currentSpeed *= snowSpeedMultiplier;
        }

        if (isOnMovingPlatform)
        {
            currentSpeed *= movingPlatformSpeedMultiplier;
        }
        rb.linearVelocity = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
        FlipSprite(moveX);

        animator.SetBool(IS_MOVING, Mathf.Abs(moveX) > 0.01f);
    }

    private void HandleJump()
    {
        if (isDucking)
        {
            jumpRequested = false;
            return;
        }
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
        animator.SetBool(IS_DUCKING, isDucking);
    }

    private void FlipSprite(float moveX)
    {
        if (moveX < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveX > 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
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
        int otherLayer = collision.gameObject.layer;

        if (((1 << otherLayer) & groundLayers) == 0)
            return;

        isGrounded = false;
        isTouchingWallLeft = false;
        isTouchingWallRight = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
            }
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
        OnHealthChanged?.Invoke(health);

        if (health <= 0)
        {
            SceneManager.LoadScene(0);
            PlayerRegistry.Instance.Clear();
            return;
        }

        isInvincible = true;
        isHurt = true;

        rb.linearVelocity = Vector2.zero;
        Vector2 knockbackDir = new Vector2(Mathf.Sign(hitDirection.x), 1f).normalized;

        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        audioSource.PlayOneShot(hurtSfx);

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

    public void SetOnMovingPlatform(bool value)
    {
        isOnMovingPlatform = value;
    }

    public void CancelJump()
    {
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -1f);
        }
    }

    public void Bounce(Vector2 normal, float bounceForce)
    {
        rb.AddForce(-normal * bounceForce);
    }
}