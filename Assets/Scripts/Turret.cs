using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [SerializeField] protected float turretRange = 15f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected LineRenderer rangeIndicator;   // always-visible circle
    [SerializeField] protected LineRenderer shapeIndicator;   // turret-specific cone/line
    [SerializeField] protected Transform target;
    protected float fireCountdown = 0f;

    protected virtual void Awake()
    {
        SetupLine(rangeIndicator, Color.red, 0.05f);
        SetupLine(shapeIndicator, Color.yellow, 0.05f);
        DrawRangeCircle();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (target == null) return;
        if (Vector3.Distance(transform.position, target.position) > turretRange) return;

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public abstract void Shoot();

    protected void SetupLine(LineRenderer lr, Color color, float width)
    {
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.sortingOrder = -1; // stays behind sprites, same fix as before
    }

    protected void DrawRangeCircle(int segments = 36)
    {
        rangeIndicator.loop = true;
        rangeIndicator.positionCount = segments;
        float step = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector3 point = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * turretRange;
            rangeIndicator.SetPosition(i, point);
        }
    }

    protected void DrawCone(LineRenderer lr, float halfAngle, int segments)
    {
        lr.loop = true;
        lr.positionCount = segments + 2;
        float facingAngle = transform.rotation.eulerAngles.z;
        float startAngle = facingAngle - halfAngle;
        float angleStep = (halfAngle * 2f) / segments;

        lr.SetPosition(0, transform.position);
        for (int i = 0; i <= segments; i++)
        {
            float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
            Vector3 point = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * turretRange;
            lr.SetPosition(i + 1, point);
        }
    }

    protected void DrawLine(LineRenderer lr)
    {
        lr.loop = false;
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.right * turretRange);
    }

    protected bool DetectInCone(float halfAngle)
    {
        if (target == null) return false;

        Vector2 dir = (Vector2)target.position - (Vector2)transform.position;
        if (dir.magnitude > turretRange) return false;

        float angleToPlayer = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float diff = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, angleToPlayer);
        return Mathf.Abs(diff) < halfAngle;
    }

    protected bool DetectAligned(float alignmentThreshold)
    {
        if (target == null) return false;

        Vector2 dir = (Vector2)target.position - (Vector2)transform.position;
        if (dir.magnitude > turretRange) return false;

        Vector2 dirToPlayer = dir.normalized;
        Vector2 turretForward = transform.right;
        float alignment = Vector2.Dot(turretForward, dirToPlayer);
        return alignment >= alignmentThreshold;
    }
}