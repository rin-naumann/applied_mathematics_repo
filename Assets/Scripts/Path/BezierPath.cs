using UnityEngine;

public class BezierPath : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] controlPoints;   // size 1 = quadratic, size 2 = cubic
    [SerializeField] private Transform target;            // same Transform on both paths
    [SerializeField] private int lengthSamples = 100;
    [SerializeField] private Color gizmoColor = Color.cyan;

    private float[] cumulative;   // cumulative[i] = curve length from t=0 to t=i/lengthSamples

    public float Length { get; private set; }
    public Vector3 SpawnPosition => spawnPoint.position;

    private void Awake() => BuildLengthTable();

    private bool IsValid() =>
        spawnPoint != null && target != null && controlPoints != null &&
        (controlPoints.Length == 1 || controlPoints.Length == 2) &&
        System.Array.TrueForAll(controlPoints, c => c != null);

    public Vector3 Evaluate(float t)
    {
        Vector3 p0 = spawnPoint.position;
        Vector3 pEnd = target.position;

        if (controlPoints.Length == 1)
            return BezierMath.Quadratic(p0, controlPoints[0].position, pEnd, t);

        return BezierMath.Cubic(p0, controlPoints[0].position, controlPoints[1].position, pEnd, t);
    }

    [ContextMenu("Rebuild Length Table")]
    private void BuildLengthTable()
    {
        if (!IsValid())
        {
            Debug.LogError($"{name}: needs a spawn point, a target, and 1 (quadratic) or 2 (cubic) control points.");
            enabled = false;
            return;
        }

        cumulative = new float[lengthSamples + 1];
        Vector3 previous = Evaluate(0f);
        float sum = 0f;

        for (int i = 1; i <= lengthSamples; i++)
        {
            Vector3 p = Evaluate((float)i / lengthSamples);
            sum += (p - previous).magnitude;
            cumulative[i] = sum;
            previous = p;
        }
        Length = sum;
    }

    public float DistanceToT(float distance)
    {
        distance = Mathf.Clamp(distance, 0f, Length);

        int lo = 0, hi = lengthSamples;
        while (hi - lo > 1)
        {
            int mid = (lo + hi) / 2;
            if (cumulative[mid] <= distance) lo = mid;
            else hi = mid;
        }

        float segment = cumulative[lo + 1] - cumulative[lo];
        float fraction = segment > 0f ? (distance - cumulative[lo]) / segment : 0f;
        return (lo + fraction) / lengthSamples;
    }

    private void OnDrawGizmos()
    {
        if (!IsValid()) return;

        Gizmos.color = gizmoColor;
        Vector3 previous = Evaluate(0f);
        const int steps = 40;
        for (int i = 1; i <= steps; i++)
        {
            Vector3 p = Evaluate((float)i / steps);
            Gizmos.DrawLine(previous, p);
            previous = p;
        }

        Gizmos.color = Color.yellow;
        foreach (Transform c in controlPoints)
        {
            Gizmos.DrawWireSphere(c.position, 0.25f);
            Gizmos.DrawLine(spawnPoint.position, c.position);
        }
        Gizmos.DrawLine(controlPoints[controlPoints.Length - 1].position, target.position);
    }
}