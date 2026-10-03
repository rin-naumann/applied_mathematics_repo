using UnityEngine;

public class ConeIndicator : TurretIndicator
{
    [SerializeField] private int coneSegments = 12;

    protected override void Draw()
    {
        IConeTurret cone = GetComponentInParent<IConeTurret>();
        if (cone == null)
        {
            Debug.LogError($"{name}: parent turret doesn't implement IConeTurret.");
            return;
        }

        float half = cone.HalfAngle;
        float step = (half * 2f) / coneSegments;

        lr.loop = true;
        lr.positionCount = coneSegments + 2;
        lr.SetPosition(0, Vector3.zero);
        for (int i = 0; i <= coneSegments; i++)
        {
            float a = (-half + step * i) * Mathf.Deg2Rad;
            lr.SetPosition(i + 1, new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * ToLocal(turret.Range));
        }
    }
}