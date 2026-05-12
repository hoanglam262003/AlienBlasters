using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class ToggleLock : MonoBehaviour, IInteractable
{
    [SerializeField] private Key requiredKey;
    [SerializeField] private UnityEvent OnUnlocked;

    private SpriteRenderer spriteRenderer;
    private bool unlocked;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        unlocked = false;
        spriteRenderer.color = Color.white;
    }

    public void Interact(Player player)
    {
        if (unlocked)
            return;

        var inventory = player.GetInventory();

        var currentItem = inventory.GetCurrentItem();

        if (currentItem is not Key currentKey)
        {
            return;
        }

        if (currentKey != requiredKey)
        {
            return;
        }
        Toggle();

        inventory.UseCurrentItem(player);
    }

    public void Toggle()
    {
        unlocked = !unlocked;

        spriteRenderer.color = Color.gray;
        if (unlocked)
        {
            OnUnlocked?.Invoke();
        }
    }
}