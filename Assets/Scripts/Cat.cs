using UnityEngine;

public class Cat : Enemy
{
    [SerializeField] private CatBomb catBombPrefab;
    [SerializeField] private Transform firePoint;

    private void Start()
    {
        var shootAnimationWrapper = GetComponentInChildren<ShootAnimationWrapper>();
        shootAnimationWrapper.OnShoot += SpawnCatBomb;
    }

    private void SpawnCatBomb()
    {
        var catBomb = Instantiate(catBombPrefab, firePoint);
        catBomb.Launch(Vector2.up + Vector2.left);
    }
}
