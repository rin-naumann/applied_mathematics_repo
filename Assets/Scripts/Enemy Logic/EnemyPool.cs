using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private IObjectPool<Enemy> pool;

    private void Awake()
    {
        pool = new ObjectPool<Enemy>(
            createFunc: CreateEnemy,
            actionOnGet: e => e.gameObject.SetActive(true),
            actionOnRelease: e => e.gameObject.SetActive(false),
            actionOnDestroy: e => Destroy(e.gameObject),
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    private Enemy CreateEnemy()
    {
        Enemy e = Instantiate(enemyPrefab, transform);
        e.SetPool(pool);
        return e;
    }

    public Enemy Spawn(BezierPath path, float speed)
    {
        Enemy e = pool.Get();
        e.Init(path, speed);
        return e;
    }
}