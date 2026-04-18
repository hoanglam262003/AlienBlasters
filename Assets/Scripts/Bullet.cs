using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private int damage = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        var brick = collision.collider.GetComponent<Brick>();
        if (brick != null)
        {
            brick.TakeBulletDamage();
        }

        Destroy(gameObject);
    }
}
