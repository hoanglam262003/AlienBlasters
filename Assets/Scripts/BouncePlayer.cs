using UnityEngine;

public class BouncePlayer : MonoBehaviour
{
    [SerializeField]
    private bool onlyFromTop;
    [SerializeField]
    private float bounceForce = 400f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onlyFromTop && Vector2.Dot(collision.contacts[0].normal, Vector2.down) < 0.5f)
            return;
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.Bounce(collision.contacts[0].normal, bounceForce);
        }
    }
}
