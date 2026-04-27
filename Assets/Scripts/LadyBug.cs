using Assets.Scripts.Interfaces;
using UnityEngine;

public class LadyBug : Enemy, ITakeLaserDamage
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float raycastDistance = 0.2f;
    [SerializeField]
    private LayerMask forwardRaycastLayerMask;

    private Vector2 direction = Vector2.left;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        maxHP = 2;
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGroundInFront();
        CheckInFront();
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }

    private void CheckInFront()
    {
        Vector2 offset = direction * col.bounds.extents.x;
        Vector2 origin = (Vector2)transform.position + offset;
        var hits = Physics2D.RaycastAll(origin, direction, raycastDistance, forwardRaycastLayerMask);
        foreach (var h in hits)
        {
            if (h.collider != null && h.collider.gameObject != gameObject)
            {
                direction *= -1;
                spriteRenderer.flipX = direction.x > 0;
                break;
            }
        }
    }

    private void CheckGroundInFront()
    {
        bool canWalking = false;
        var downOrigin = GetDownPosition(col);
        var downHits = Physics2D.RaycastAll(downOrigin, Vector2.down, raycastDistance);
        foreach (var h in downHits)
        {
            if (h.collider != null && h.collider.gameObject != gameObject)
            {
                canWalking = true;
                break;
            }
        }
        if (canWalking == false)
        {
            direction *= -1;
            spriteRenderer.flipX = direction.x > 0;
            return;
        }
    }

    private Vector2 GetDownPosition(Collider2D collider)
    {
        var bounds = collider.bounds;
        if (direction == Vector2.left)
        {
            return new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y);
        }
        else
        {
            return new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y);
        }
    }

    public void TakeLaserDamage()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
