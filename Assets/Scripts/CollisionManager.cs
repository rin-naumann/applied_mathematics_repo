using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }

    private static readonly List<ICollidable> collidables = new List<ICollidable>();

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public static void Register(ICollidable collidable)
    {
        if (collidables.Contains(collidable)) return;
        collidables.Add(collidable);
    }

    public static void Unregister(ICollidable collidable)
    {
        collidables.Remove(collidable);
    }

    void LateUpdate()
    {
        for (int i = 0; i < collidables.Count; i++)
            collidables[i].Collider.Recalculate();

        for (int i = 0; i < collidables.Count; i++)
        {
            for (int j = i + 1; j < collidables.Count; j++)
            {
                var a = collidables[i];
                var b = collidables[j];
                if (!Overlaps(a, b)) continue;
                a.OnColliderEnter(b);
                b.OnColliderEnter(a);
            }
        }
    }

    bool Overlaps(ICollidable a, ICollidable b)
    {
        return a.Collider.min.x < b.Collider.max.x && a.Collider.max.x > b.Collider.min.x &&
               a.Collider.min.y < b.Collider.max.y && a.Collider.max.y > b.Collider.min.y;
    }
}