using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<IItem> items = new List<IItem>();
    private int currentIndex = -1;

    public void AddItem(IItem item)
    {
        if (currentIndex >= 0)
        {
            SetItemActive(items[currentIndex], false);
        }

        items.Add(item);
        currentIndex = items.Count - 1;

        SetItemActive(item, true);

        Debug.Log($"Picked item: {item.GetType().Name}");
    }

    public void UseCurrentItem(Player player)
    {
        if (currentIndex < 0) return;

        var item = items[currentIndex];

        item.Use(player);

        var mono = item as MonoBehaviour;
        if (mono != null)
        {
            Destroy(mono.gameObject);
        }

        items.RemoveAt(currentIndex);

        if (items.Count == 0)
        {
            currentIndex = -1;
            return;
        }

        currentIndex = Mathf.Clamp(currentIndex - 1, 0, items.Count - 1);
        SetItemActive(items[currentIndex], true);
    }

    public void SwapItem()
    {
        if (items.Count <= 1) return;

        SetItemActive(items[currentIndex], false);

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = items.Count - 1;
        }

        SetItemActive(items[currentIndex], true);

        Debug.Log($"Swapped to: {items[currentIndex].GetType().Name}");
    }

    public bool HasItem<T>() where T : IItem
    {
        foreach (var item in items)
        {
            if (item is T) return true;
        }
        return false;
    }

    private void SetItemActive(IItem item, bool active)
    {
        var mono = item as MonoBehaviour;
        if (mono == null) return;

        var sr = mono.GetComponent<SpriteRenderer>();
        var col = mono.GetComponent<Collider2D>();

        if (sr != null) sr.enabled = active;
        if (col != null) col.enabled = active;
    }
}