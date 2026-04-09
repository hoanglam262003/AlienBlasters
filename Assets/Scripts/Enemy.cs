using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private bool ignoreFromTop = true;
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (ignoreFromTop && Vector2.Dot(collision.contacts[0].normal, Vector2.down) > 0.5)
            return;
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            Vector2 hitDirection = (player.transform.position - transform.position).normalized;

            player.GetHit(hitDirection);
        }
    }
}
