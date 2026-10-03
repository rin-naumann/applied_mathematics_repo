using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [SerializeField] protected float turretRange = 15f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected BulletPool bulletPool;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] private Transform baseTransform;   // the shared target location

    protected Transform target;          // chosen every frame by AcquireTarget()
    protected float fireCountdown = 0f;

    private readonly List<Enemy> inRange = new List<Enemy>();

    public float Range => turretRange;   // read by the indicator scripts

    protected virtual void Awake()
    {
        if (baseTransform == null)
        {
            GameObject baseObj = GameObject.FindGameObjectWithTag("Base");
            if (baseObj != null) baseTransform = baseObj.transform;
            else Debug.LogError($"{name}: no Base assigned and no object tagged 'Base' found!");
        }
    }

    protected virtual void Update()
    {
        AcquireTarget();
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public abstract void Shoot();

    private void AcquireTarget()
    {
        target = null;
        if (baseTransform == null) return;

        // 1. every live creature within range goes into the list
        inRange.Clear();
        Vector2 turretPos = transform.position;
        float rangeSqr = turretRange * turretRange;
        foreach (Enemy e in Enemy.Active)
        {
            if (((Vector2)e.transform.position - turretPos).sqrMagnitude <= rangeSqr)
                inRange.Add(e);
        }

        // 2 + 3. compare distance to the base, keep the closest as the target
        Vector2 basePos = baseTransform.position;
        float closestSqr = float.MaxValue;
        foreach (Enemy e in inRange)
        {
            float d = ((Vector2)e.transform.position - basePos).sqrMagnitude;
            if (d < closestSqr)
            {
                closestSqr = d;
                target = e.transform;
            }
        }
    }

    protected bool DetectInCone(float halfAngle)
    {
        if (target == null) return false;

        Vector2 dir = (Vector2)target.position - (Vector2)transform.position;
        if (dir.magnitude > turretRange) return false;

        float angleToTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float diff = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, angleToTarget);
        return Mathf.Abs(diff) < halfAngle;
    }

    protected bool DetectAligned(float alignmentThreshold)
    {
        if (target == null) return false;

        Vector2 dir = (Vector2)target.position - (Vector2)transform.position;
        if (dir.magnitude > turretRange) return false;

        float alignment = Vector2.Dot((Vector2)transform.right, dir.normalized);
        return alignment >= alignmentThreshold;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }
}