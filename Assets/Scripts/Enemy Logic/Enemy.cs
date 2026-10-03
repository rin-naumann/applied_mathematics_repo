using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public static readonly List<Enemy> Active = new List<Enemy>();

    [SerializeField] private float hitRadius = 0.4f;
    [SerializeField] private int damageToPlayer = 1;
    public float HitRadius => hitRadius;

    private IObjectPool<Enemy> pool;
    private BezierPath path;
    private float speed;
    private float distanceTravelled;

    private void OnEnable()  => Active.Add(this);
    private void OnDisable() => Active.Remove(this);

    public void SetPool(IObjectPool<Enemy> ownerPool) => pool = ownerPool;

    public void Init(BezierPath followPath, float moveSpeed)
    {
        path = followPath;
        speed = moveSpeed;
        distanceTravelled = 0f;
        transform.position = path.Evaluate(0f);
    }

    private void Update()
    {
        distanceTravelled += speed * Time.deltaTime;

        if (distanceTravelled >= path.Length)
        {
            ReachTarget();
            return;
        }

        float t = path.DistanceToT(distanceTravelled);
        transform.position = path.Evaluate(t);
    }

    private void ReachTarget()
    {
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.TakeDamage(damageToPlayer);

        pool.Release(this);
    }

    public void Die()
    {
        if (CoinPool.Instance != null)
            CoinPool.Instance.Spawn(transform.position);

        pool.Release(this);
    }
}