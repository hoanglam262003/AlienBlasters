using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            Vector2 hitDirection = (player.transform.position - transform.position).normalized;

            player.GetHit(hitDirection);
        }
    }
}
