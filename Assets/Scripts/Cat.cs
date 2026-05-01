using UnityEngine;

public class Cat : Enemy
{
    [SerializeField] private GameObject catBombPrefab;
    [SerializeField] private Transform firePoint;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCatBomb), 3f, 3f);
    }

    private void SpawnCatBomb()
    {
        Instantiate(catBombPrefab, firePoint);
    }
}
