using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float hitDistance = 0.1f;

    private PlayerController target;
    private float timeAlive = 0f;
    private MissilePool pool;

    public void SetPool(MissilePool pool)
    {
        this.pool = pool;
    }

    public void Init(PlayerController player)
    {
        target = player;
        timeAlive = 0f;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime || target == null)
        {
            pool.Release(this);
            return;
        }
        
        Vector3 dir = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        transform.position += transform.up * speed * Time.deltaTime;

        if (dir.magnitude <= hitDistance) HitTarget();
    }

    private void HitTarget()
    {
        target.TakeHit();
        pool.Release(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitDistance);
    }
}
