using UnityEngine;
using UnityEngine.Serialization;

public class ShotgunTurret : Turret, IConeTurret
{
    [FormerlySerializedAs("deltaAngle")]
    [SerializeField] private float halfAngle = 30f;
    [SerializeField] private int pelletCount = 6;
    [SerializeField] private float pelletSpread = 25f;

    public float HalfAngle => halfAngle;

    protected override void Update()
    {
        base.Update();

        if (!DetectInCone(halfAngle)) return;

        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    public override void Shoot()
    {
        float baseAngle = transform.rotation.eulerAngles.z;
        float startAngle = baseAngle - pelletSpread / 2f;
        float angleStep = pelletCount > 1 ? pelletSpread / (pelletCount - 1) : 0f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            bulletPool.Spawn(firePoint.position, dir, bulletSpeed);
        }
    }
}