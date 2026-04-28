using Assets.Scripts.Interfaces;
using UnityEngine;

public class Key : MonoBehaviour, IItem
{
    public void Use(Player player)
    {
        Debug.Log("Key used");
    }

    public void OnPicked(Player player)
    {
        transform.SetParent(player.transform);
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            var item = GetComponent<IItem>();

            player.GetInventory().AddItem(item);

            OnPicked(player);
        }
    }
}