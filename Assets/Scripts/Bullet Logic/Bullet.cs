using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float maxLifeTime = 5f;
    [SerializeField] private float hitRadius = 0.3f;

    private IObjectPool<Bullet> pool;
    private float speed;
    private float lifeRemaining;
    private Vector3 previousPosition;

    public void SetPool(IObjectPool<Bullet> ownerPool) => pool = ownerPool;

    // Runs every time the bullet leaves the pool (Start only runs once per object).
    public void Init(Vector3 position, Vector3 direction, float bulletSpeed)
    {
        transform.position = position;
        transform.right = direction;
        speed = bulletSpeed;
        lifeRemaining = maxLifeTime;
        previousPosition = position;
    }

    private void Update()
    {
        previousPosition = transform.position;
        transform.position += transform.right * speed * Time.deltaTime;

        lifeRemaining -= Time.deltaTime;
        if (lifeRemaining <= 0f)
        {
            Despawn();
            return;
        }

        if (TryHitEnemy())
            Despawn();
    }

    private bool TryHitEnemy()
    {
        Vector2 a = previousPosition;
        Vector2 b = transform.position;

        // backwards, because Die() removes the enemy from Enemy.Active
        for (int i = Enemy.Active.Count - 1; i >= 0; i--)
        {
            Enemy enemy = Enemy.Active[i];
            Vector2 enemyPos = enemy.transform.position;

            Vector2 closest = ClosestPointOnSegment(a, b, enemyPos);
            float reach = hitRadius + enemy.HitRadius;

            if ((closest - enemyPos).sqrMagnitude <= reach * reach)
            {
                enemy.Die();
                return true;   // one bullet, one hit
            }
        }
        return false;
    }

    private static Vector2 ClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 point)
    {
        Vector2 segment = b - a;
        float lengthSqr = segment.sqrMagnitude;
        if (lengthSqr == 0f) return a;

        float t = Mathf.Clamp01(Vector2.Dot(point - a, segment) / lengthSqr);
        return a + segment * t;
    }

    private void Despawn() => pool.Release(this);
}