using System;
using UnityEngine;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    
    private float lastShootTime;

    public void TryShoot()
    {
        if (Time.time < lastShootTime + fireRate)
        {
            return;
        }

        Shoot();
        lastShootTime = Time.time;
    }

    private void Shoot()
    {
        Bullet bullet = PoolManager.Instance.GetBullet();
        bullet.transform.position = firePoint.position;

        float direction = transform.root.localScale.x > 0 ? 1f : -1f;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(direction * 10f, 0f);

        bullet.transform.right = new Vector2(direction, 0f);
        bullet.transform.Rotate(0, 0, -90f);
    }
}
