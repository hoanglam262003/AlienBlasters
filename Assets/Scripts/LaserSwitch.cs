using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private UnityEvent on;
    [SerializeField] private UnityEvent off;

    private SpriteRenderer spriteRenderer;
    private bool isOn;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        Toggle();
    }

    private void Toggle()
    {
        isOn = !isOn;

        if (isOn)
        {
            on.Invoke();
            spriteRenderer.sprite = rightSprite;
        }
        else
        {
            off.Invoke();
            spriteRenderer.sprite = leftSprite;
        }
    }
}
