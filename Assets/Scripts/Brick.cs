using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private ParticleSystem brickBreakParticle;
    [SerializeField] private float laserDestroyTime = 1f;

    private SpriteRenderer spriteRenderer;
    private float takeDamageTime;
    private float originalColorTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player == null)
        {
            return;
        }
        Vector2 normal = collision.GetContact(0).normal;
        float dot = Vector2.Dot(normal, Vector2.up);
        if (dot > 0.5f)
        {
            player.CancelJump();
            Explode();
        }
    }

    public void TakeLaserDamage()
    {
        spriteRenderer.color = Color.red;
        originalColorTime = Time.time + 0.1f;
        takeDamageTime += Time.deltaTime;
        if (takeDamageTime >= laserDestroyTime)
        {
            Explode();
        }
    }

    private void Update()
    {
        if (originalColorTime > 0 && originalColorTime <= Time.time)
        {
            originalColorTime = 0;
            spriteRenderer.color = Color.white;
        }
    }

    private void Explode()
    {
        Instantiate(brickBreakParticle, transform.localPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
