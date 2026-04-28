using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Stack<IItem> items = new Stack<IItem>();

    public void AddItem(IItem item)
    {
        if (items.Count > 0)
        {
            SetItemActive(items.Peek(), false);
        }

        items.Push(item);

        SetItemActive(item, true);

        Debug.Log($"Picked item: {item.GetType().Name}");
    }

    public void UseTopItem(Player player)
    {
        if (items.Count == 0) return;

        var item = items.Pop();

        item.Use(player);

        var mono = item as MonoBehaviour;
        if (mono != null)
        {
            Destroy(mono.gameObject);
        }

        if (items.Count > 0)
        {
            SetItemActive(items.Peek(), true);
        }
    }

    public IItem PeekItem()
    {
        return items.Count > 0 ? items.Peek() : null;
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