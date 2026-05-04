using UnityEngine;

public class Cat : Enemy
{
    [SerializeField] private CatBomb catBombPrefab;
    [SerializeField] private Transform firePoint;
    private CatBomb catBomb;

    private void Start()
    {
        SpawnCatBomb();
        var shootAnimationWrapper = GetComponentInChildren<ShootAnimationWrapper>();
        shootAnimationWrapper.OnShoot += ThrowCatBomb;
        shootAnimationWrapper.OnReload += SpawnCatBomb;
    }

    private void SpawnCatBomb()
    {
        if (catBomb == null)
        {
            catBomb = Instantiate(catBombPrefab, firePoint);
        }
    }

    private void ThrowCatBomb()
    {
        catBomb.Launch(Vector2.up + Vector2.left);
        catBomb = null;
    }
}
