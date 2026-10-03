using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int defaultCapacity = 30;
    [SerializeField] private int maxSize = 300;

    private IObjectPool<Bullet> pool;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: b => b.gameObject.SetActive(true),
            actionOnRelease: b => b.gameObject.SetActive(false),
            actionOnDestroy: b => Destroy(b.gameObject),
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    private Bullet CreateBullet()
    {
        Bullet b = Instantiate(bulletPrefab, transform);
        b.SetPool(pool);   // the bullet needs to know where to return itself
        return b;
    }

    public Bullet Spawn(Vector3 position, Vector3 direction, float speed)
    {
        Bullet b = pool.Get();
        b.Init(position, direction, speed);
        return b;
    }
}