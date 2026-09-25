using UnityEngine;

public class SniperTurret : Turret
{
    [SerializeField] private float alignmentThreshold = 0.98f;

    protected override void Update()
{
    base.Update();
    DrawLine(shapeIndicator);

    bool aligned = DetectAligned(alignmentThreshold);
    Debug.Log(aligned + " | " + target.position);
    if (!aligned) return;

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