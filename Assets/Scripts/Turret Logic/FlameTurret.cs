using UnityEngine;
using UnityEngine.Serialization;

public class FlameTurret : Turret, IConeTurret
{
    [FormerlySerializedAs("deltaAngle")]
    [SerializeField] private float halfAngle = 20f;

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
        bulletPool.Spawn(firePoint.position, firePoint.right, bulletSpeed);
    }
}