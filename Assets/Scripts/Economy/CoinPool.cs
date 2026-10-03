using UnityEngine;
using UnityEngine.Pool;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance { get; private set; }

    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 200;

    private IObjectPool<Coin> pool;

    private void Awake()
    {
        Instance = this;

        pool = new ObjectPool<Coin>(
            createFunc: CreateCoin,
            actionOnGet: c => c.gameObject.SetActive(true),
            actionOnRelease: c => c.gameObject.SetActive(false),
            actionOnDestroy: c => Destroy(c.gameObject),
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    private Coin CreateCoin()
    {
        Coin c = Instantiate(coinPrefab, transform);
        c.SetPool(pool);
        return c;
    }

    public Coin Spawn(Vector3 position)
    {
        Coin c = pool.Get();
        c.Init(position);
        return c;
    }
}