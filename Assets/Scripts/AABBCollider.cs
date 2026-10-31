using UnityEngine;

public class AABBCollider : MonoBehaviour
{
    [SerializeField] private float length;
    [SerializeField] private float width;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    void Update()
    {
        Recalculate();

        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), Color.white);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), Color.white);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), Color.white);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), Color.white);
    }

    public void Recalculate()
    {
        Vector2 half = new Vector2(width, length) * 0.5f;
        Vector2 center = transform.position;
        max = center + half;
        min = center - half;
    }
}