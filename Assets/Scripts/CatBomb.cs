using UnityEngine;

public class CatBomb : MonoBehaviour
{
    [SerializeField] private float forceValue = 300f;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Launch(Vector2 direction)
    {
        transform.SetParent(null);
        rb.AddForce(direction * forceValue);
    }
}
