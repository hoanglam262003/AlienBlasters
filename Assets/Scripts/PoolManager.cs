using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    private ObjectPool<Bullet> pool;

    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
    
    public Bullet GetBullet()
    {
        return pool.Get();
    }
}
