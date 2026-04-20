using UnityEngine;

public class Dog : Enemy
{
    [SerializeField] private BoxCollider2D bodyCollider;
    [SerializeField] private Vector2 standSize;
    [SerializeField] private Vector2 standOffset;
    [SerializeField] private Vector2 crouchSize;
    [SerializeField] private Vector2 crouchOffset;

    public override bool UseDamageReceiver => true;
    protected override void Awake()
    {
        maxHP = 10;
        base.Awake();
    }

    public void Shoot()
    {

    }
    public void SetStand()
    {
        bodyCollider.size = standSize;
        bodyCollider.offset = standOffset;
    }

    public void SetCrouch()
    {
        bodyCollider.size = crouchSize;
        bodyCollider.offset = crouchOffset;
    }
}
