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
        var inventory = player.GetInventory();

        if (!inventory.HasItem<Key>())
            return;

        if (unlocked)
            return;

        Toggle();

        inventory.UseTopItem(player);
    }
    public void Toggle()
    {
        unlocked = !unlocked;
        spriteRenderer.color = unlocked ? Color.white : Color.gray;
    }
}
