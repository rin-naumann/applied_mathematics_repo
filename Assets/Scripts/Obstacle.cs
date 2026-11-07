using UnityEngine;

[RequireComponent(typeof(PerspectiveObject))]
[RequireComponent(typeof(AABBCollider))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    [SerializeField] private float despawnZ = -150f;

    public PerspectiveObject Perspective { get; private set; }
    public float Z => Perspective.position.z;
    public bool IsPastPlayer => Z < despawnZ;

    private void Awake()
    {
        Perspective = GetComponent<PerspectiveObject>();
    }

    public void Init(Vector3 startPosition)
    {
        Perspective.position = startPosition;
    }

    // Called once per frame by ObstacleManager
    public void Tick(float deltaTime)
    {
        Perspective.position.z -= speed * deltaTime;
    }

        private void OnEnable()
    {
        if (ObstacleManager.Instance != null)
            ObstacleManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (ObstacleManager.Instance != null)
            ObstacleManager.Instance.Unregister(this);
    }
}