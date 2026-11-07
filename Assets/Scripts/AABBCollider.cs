using System;
using UnityEngine;

[RequireComponent(typeof(PerspectiveObject))]
public class AABBCollider : MonoBehaviour
{
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _width = 1f;

    public event Action<AABBCollider> Collided;

    private PerspectiveObject perspective;

    private Vector2 Center => new Vector2(perspective.position.x, perspective.position.y);
    private Vector2 HalfSize => new Vector2(_width, _length) * 0.5f;

    public Vector2 min => Center - HalfSize;
    public Vector2 max => Center + HalfSize;

    public float Z => perspective.position.z;
    public float PreviousZ { get; private set; }
    public bool HasPreviousZ { get; private set; }

    private void Awake()
    {
        perspective = GetComponent<PerspectiveObject>();
    }

    private void OnEnable()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        HasPreviousZ = false;
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.Unregister(this);
    }

    public void RecordZ()
    {
        PreviousZ = Z;
        HasPreviousZ = true;
    }

    private void Update()
    {
        float s = perspective.Scale;
        Vector2 lo = min * s;
        Vector2 hi = max * s;

        Debug.DrawLine(new Vector3(lo.x, hi.y), new Vector3(hi.x, hi.y), Color.blueViolet);
        Debug.DrawLine(new Vector3(hi.x, hi.y), new Vector3(hi.x, lo.y), Color.blueViolet);
        Debug.DrawLine(new Vector3(hi.x, lo.y), new Vector3(lo.x, lo.y), Color.blueViolet);
        Debug.DrawLine(new Vector3(lo.x, lo.y), new Vector3(lo.x, hi.y), Color.blueViolet);
    }

    public void OnCollision(AABBCollider other)
    {
        Debug.Log($"Hit {other.name}");
        Collided?.Invoke(other);
    }

    private void OnDrawGizmos()
    {
        Vector3 center;
        float scale;

        if (Application.isPlaying && perspective != null)
        {
            scale = perspective.Scale;
            center = new Vector3(perspective.position.x, perspective.position.y, 0f) * scale;
        }
        else
        {
            scale = 1f;
            center = transform.position;
        }

        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireCube(center, new Vector3(_width * scale, _length * scale, 0f));
    }
}