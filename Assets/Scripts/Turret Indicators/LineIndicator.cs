using UnityEngine;

public class LineIndicator : TurretIndicator
{
    protected override void Draw()
    {
        lr.loop = false;
        lr.positionCount = 2;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.right * ToLocal(turret.Range));
    }
}