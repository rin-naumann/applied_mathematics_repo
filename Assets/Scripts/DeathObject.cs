using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathObject : MonoBehaviour, ICollidable
{
    public AABBCollider Collider { get; private set; }

    void Awake() => Collider = GetComponent<AABBCollider>();
    void OnEnable() => CollisionManager.Register(this);
    void OnDisable() => CollisionManager.Unregister(this);

    public void OnColliderEnter(ICollidable other)
    {
        if (other is PlayerController) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}