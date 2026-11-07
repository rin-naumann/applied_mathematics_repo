using UnityEngine;
using System.Collections.Generic;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance => instance;
    private static CollisionManager instance;

    private List<AABBCollider> colliders = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Register(AABBCollider collider)
    {
        if (colliders.Contains(collider)) return;
        colliders.Add(collider);
    }
    
    public void Unregister(AABBCollider collider)
    {
        if (!colliders.Contains(collider)) return;
        colliders.Remove(collider);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            var colliderA = colliders[i];
            for (int j = i + 1; j < colliders.Count; j++)
            {
                var colliderB = colliders[j];
                if (Overlaps(colliderA, colliderB))
                {
                    colliderA.OnCollision(colliderB);
                    colliderB.OnCollision(colliderA);
                }
            }
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].RecordZ();
        }
    }

    bool Overlaps(AABBCollider a, AABBCollider b)
    {
        if (!a.HasPreviousZ || !b.HasPreviousZ) return false; // first frame: nothing to compare against

        return CrossedInZ(a, b) &&
            a.min.x <= b.max.x && b.min.x <= a.max.x &&
            a.min.y <= b.max.y && b.min.y <= a.max.y;
    }

    // True only on the frame where the z-gap between a and b changes sign (or reaches zero).
    bool CrossedInZ(AABBCollider a, AABBCollider b)
    {
        float before = a.PreviousZ - b.PreviousZ;
        float now = a.Z - b.Z;
        return (before > 0f && now <= 0f) || (before < 0f && now >= 0f);
    }
}
