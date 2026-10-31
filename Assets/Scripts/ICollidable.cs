public interface ICollidable
{
    AABBCollider Collider { get; }
    void OnColliderEnter(ICollidable other);
}
