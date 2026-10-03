using UnityEngine;

public class RangeIndicator : TurretIndicator
{
    [SerializeField] private int segments = 36;

    protected override void Draw()
    {
        lr.loop = true;
        lr.positionCount = segments;
        float step = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float a = step * i * Mathf.Deg2Rad;
            lr.SetPosition(i, new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * ToLocal(turret.Range));
        }
    }
}