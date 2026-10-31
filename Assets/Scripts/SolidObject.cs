using UnityEngine;

public class SolidObject : MonoBehaviour, ICollidable
{
    public AABBCollider Collider { get; private set; }

    void Awake()
    {
        Collider = GetComponent<AABBCollider>();
    }

    void OnDisable() => CollisionManager.Unregister(this);
    void OnEnable() => CollisionManager.Register(this);
    public void OnColliderEnter(ICollidable other) {}
}
