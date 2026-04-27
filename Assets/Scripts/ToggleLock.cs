using Assets.Scripts.Interfaces;
using UnityEngine;

public class ToggleLock : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    private bool unlocked;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        unlocked = false;
        spriteRenderer.color = Color.gray;
    }
    public void Interact(Player player)
    {
        if (!player.HasKey())
        {
            return;
        }
        if (unlocked)
        {
            return;
        }
        Toggle();
        player.ConsumeKey();
        player.SetHasKey(false);
    }
    public void Toggle()
    {
        unlocked = !unlocked;
        spriteRenderer.color = unlocked ? Color.white : Color.gray;
    }
}
