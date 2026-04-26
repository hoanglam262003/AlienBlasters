using UnityEngine;

public class ToggleLock : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool unlocked;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        unlocked = false;
        spriteRenderer.color = Color.gray;
    }
    public void Toggle()
    {
        unlocked = !unlocked;
        spriteRenderer.color = unlocked ? Color.white : Color.gray;
    }
}
