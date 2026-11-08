using UnityEngine;

public class LaneGuides : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1200f;  // keep equal to Obstacle speed
    [SerializeField] private float tickSpacing = 300f;   // z distance between stripes
    [SerializeField] private int tickCount = 14;
    [SerializeField] private float nearZ = -200f;        // past the player, so lines start off-screen
    [SerializeField] private float farZ = 20000f;        // far enough to practically be the vanishing point
    [SerializeField] private float lineWidth = 0.06f;
    [SerializeField] private float tickThickness = 0.12f;
    [SerializeField] private Color color = new Color(1f, 1f, 1f, 0.35f);

    private LineRenderer[] boundaries;
    private LineRenderer[] ticks;
    private Material lineMaterial;
    private float scroll;

    private void Awake()
    {
        lineMaterial = new Material(Shader.Find("Sprites/Default"));

        boundaries = new LineRenderer[Lanes.Count + 1];
        for (int i = 0; i < boundaries.Length; i++)
            boundaries[i] = CreateLine($"Boundary {i}");

        ticks = new LineRenderer[tickCount];
        for (int i = 0; i < ticks.Length; i++)
            ticks[i] = CreateLine($"Tick {i}");
    }

    private void Update()
    {
        scroll += scrollSpeed * Time.deltaTime;

        float left = Lanes.GetX(0) - Lanes.spacing * 0.5f;
        float right = Lanes.GetX(Lanes.Count - 1) + Lanes.spacing * 0.5f;

        UpdateBoundaries(left);
        UpdateTicks(left, right);
    }

    private void UpdateBoundaries(float left)
    {
        float sNear = Scale(nearZ);
        float sFar = Scale(farZ);

        for (int i = 0; i < boundaries.Length; i++)
        {
            float x = left + i * Lanes.spacing;
            var line = boundaries[i];
            line.SetPosition(0, Project(x, nearZ));
            line.SetPosition(1, Project(x, farZ));
            line.startWidth = lineWidth * sNear;   // thick up close, thin in the distance
            line.endWidth = lineWidth * sFar;
        }
    }

    private void UpdateTicks(float left, float right)
    {
        float range = tickCount * tickSpacing;

        for (int i = 0; i < ticks.Length; i++)
        {
            // Each stripe's z decreases over time; Repeat wraps it back to the far end.
            float z = nearZ + Mathf.Repeat(i * tickSpacing - scroll, range);
            float s = Scale(z);

            var line = ticks[i];
            line.SetPosition(0, Project(left, z));
            line.SetPosition(1, Project(right, z));
            line.startWidth = line.endWidth = tickThickness * s;
        }
    }

    // Same math as PerspectiveObject.
    private static float Scale(float z)
    {
        return CameraComponent.focalLenth / (CameraComponent.focalLenth + z);
    }

    private static Vector3 Project(float x, float z)
    {
        float s = Scale(z);
        return new Vector3(x * s, Lanes.groundY * s, 0f);
    }

    private LineRenderer CreateLine(string objectName)
    {
        var go = new GameObject(objectName);
        go.transform.SetParent(transform, false);

        var line = go.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.material = lineMaterial;
        line.startColor = line.endColor = color;
        line.sortingOrder = -10000;   // behind everything: obstacles use -z, the player uses 0
        return line;
    }
}