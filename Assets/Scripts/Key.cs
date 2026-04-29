using Assets.Scripts.Interfaces;
using UnityEngine;

public class Key : MonoBehaviour, IItem
{
    private void LateUpdate()
    {
        transform.localScale = Vector3.one;
    }
    public void Use(Player player)
    {
        Debug.Log("Key used");
    }

    public void OnPicked(Player player)
    {
        Transform holdPoint = player.GetHoldItemPoint();

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
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