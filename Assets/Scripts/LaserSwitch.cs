using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private UnityEvent on;
    [SerializeField] private UnityEvent off;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb.linearVelocity.x > 0)
        {
            TurnOn();
        }
        else if (rb.linearVelocity.x < 0)
        {
            TurnOff();
        }
    }

    private void TurnOn()
    {
        spriteRenderer.sprite = rightSprite;
        on.Invoke();
    }

    private void TurnOff()
    {
        spriteRenderer.sprite = leftSprite;
        off.Invoke();
    }
}
