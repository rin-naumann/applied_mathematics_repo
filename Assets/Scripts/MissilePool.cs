using UnityEngine;
using UnityEngine.Pool;

public class MissilePool : MonoBehaviour
{
    public Missile missilePrefab;

    [Header("Pool Settings")]
    public int defaultCapacity = 20;
    public int maxSize = 100;

    private ObjectPool<Missile> missilePool;

    void Awake()
    {
        missilePool = new ObjectPool<Missile>(
            createFunc: CreateMissile,
            actionOnGet: missile => missile.gameObject.SetActive(true),
            actionOnRelease: missile => missile.gameObject.SetActive(false),
            actionOnDestroy: missile => Destroy(missile.gameObject),
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private Missile CreateMissile()
    {
        Missile missile = Instantiate(missilePrefab, transform);
        missile.SetPool(this);
        return missile;
    }

    public Missile Get(Vector3 position, PlayerController target)
    {
        Missile missile = missilePool.Get();
        missile.transform.position = position;
        missile.Init(target);
        return missile;
    }

    public void Release(Missile missile)
    {
        missilePool.Release(missile);
    }
}