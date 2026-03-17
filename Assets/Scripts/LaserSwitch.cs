using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

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
    }

    private void TurnOff()
    {
        spriteRenderer.sprite = leftSprite;
    }
}
