using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private Obstacle prefab;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 30;

    private ObjectPool<Obstacle> pool;

    private void Awake()
    {
        pool = new ObjectPool<Obstacle>(
            createFunc: () => Instantiate(prefab, transform),
            actionOnGet: o => o.gameObject.SetActive(true),
            actionOnRelease: o => o.gameObject.SetActive(false),
            actionOnDestroy: o => Destroy(o.gameObject),
            collectionCheck: true,   // throws in the editor if something is released twice
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    public Obstacle Get() => pool.Get();

    public void Release(Obstacle obstacle) => pool.Release(obstacle);
}