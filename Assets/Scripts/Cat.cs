using UnityEngine;

public class Cat : Enemy
{
    [SerializeField] private CatBomb catBombPrefab;
    [SerializeField] private Transform firePoint;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCatBomb), 3f, 3f);
    }

    private void SpawnCatBomb()
    {
        var catBomb = Instantiate(catBombPrefab, firePoint);
        catBomb.Launch(Vector2.up + Vector2.left);
    }
}
