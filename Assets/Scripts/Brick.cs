using UnityEngine;

public class Brick : MonoBehaviour
{
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
            Destroy(gameObject);
        }
    }
}
