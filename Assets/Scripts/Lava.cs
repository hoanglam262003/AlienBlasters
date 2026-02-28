using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private float damageInterval = 1f;

    private Coroutine damageCoroutine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player == null) return;

        damageCoroutine = StartCoroutine(DamageLoop(player));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player == null) return;

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageLoop(Player player)
    {
        while (true)
        {
            Vector2 hitDirection = player.transform.position - transform.position;

            player.GetHit(hitDirection);

            yield return new WaitForSeconds(damageInterval);
        }
    }
}