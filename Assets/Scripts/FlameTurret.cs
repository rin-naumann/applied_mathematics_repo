using UnityEngine;

public class FlameTurret : Turret
{
    [SerializeField] private float deltaAngle = 20f;
    [SerializeField] private int coneSegments = 12;

    protected override void Update()
    {
        base.Update();

        bool playerInCone = DetectInCone(deltaAngle);
        DrawCone(shapeIndicator, deltaAngle, coneSegments);

        if (!playerInCone) return;

        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    public override void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletObj.GetComponent<Bullet>().speed = bulletSpeed;
    }
}