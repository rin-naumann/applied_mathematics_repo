using UnityEngine;

public class SniperTurret : Turret
{
    [SerializeField] private float alignmentThreshold = 0.98f;

    protected override void Update()
    {
        base.Update();

        if (!DetectAligned(alignmentThreshold)) return;

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