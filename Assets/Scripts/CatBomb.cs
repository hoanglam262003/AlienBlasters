using UnityEngine;

public class CatBomb : MonoBehaviour
{
    [SerializeField] private float forceValue = 300f;

    private Rigidbody2D rb;
    private Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        rb.simulated = false;
        animator.enabled = false;
    }
    public void Launch(Vector2 direction)
    {
        transform.SetParent(null);
        rb.simulated = true;
        rb.AddForce(direction * forceValue);
        animator.enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Vector2 hitDir = (player.transform.position - transform.position).normalized;
            player.GetHit(hitDir);
        }

        Destroy(gameObject);
    }
}
