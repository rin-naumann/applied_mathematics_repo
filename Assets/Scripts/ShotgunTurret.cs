using UnityEngine;

public class ShotgunTurret : Turret
{
    [SerializeField] private float deltaAngle = 30f;      
    [SerializeField] private int coneSegments = 12;
    [SerializeField] private int pelletCount = 6;
    [SerializeField] private float pelletSpread = 25f;    

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
        float baseAngle = transform.rotation.eulerAngles.z;
        float startAngle = baseAngle - pelletSpread / 2f;
        float angleStep = pelletCount > 1 ? pelletSpread / (pelletCount - 1) : 0f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bulletObj.transform.right = dir;
            bulletObj.GetComponent<Bullet>().speed = bulletSpeed;
        }
    }
}