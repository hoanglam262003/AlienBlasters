using System;
using UnityEngine;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    private ObjectPool<Bullet> pool;
    private float lastShootTime;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(AddBulletToPool,
            t => t.gameObject.SetActive(true),
            t => t.gameObject.SetActive(false)
            );
    }

    private Bullet AddBulletToPool()
    {
        var shot = Instantiate(bulletPrefab).GetComponent<Bullet>();
        shot.SetPool(pool);
        return shot;
    }

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
        GameObject bullet = pool.Get().gameObject;
        bullet.transform.position = firePoint.position;

        float direction = transform.root.localScale.x > 0 ? 1f : -1f;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(direction * 10f, 0f);

        bullet.transform.right = new Vector2(direction, 0f);
        bullet.transform.Rotate(0, 0, -90f);
    }
}
