using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            transform.SetParent(player.transform);
            player.AttachKey(gameObject);
            player.SetHasKey(true);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
