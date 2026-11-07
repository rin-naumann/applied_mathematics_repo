using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance => instance;
    private static ObstacleManager instance;

    [SerializeField] private ObjectPooler pooler;

    private readonly List<Obstacle> obstacles = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    public Obstacle Spawn(Vector3 position)
    {
        var obstacle = pooler.Get();
        obstacle.Init(position);
        return obstacle;
    }

    public void Register(Obstacle obstacle)
    {
        if (obstacles.Contains(obstacle)) return;
        obstacles.Add(obstacle);
    }

    public void Unregister(Obstacle obstacle)
    {
        obstacles.Remove(obstacle);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // Backwards, because Release -> SetActive(false) -> OnDisable -> Unregister
        // removes the element from this list while we're looping over it.
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = obstacles[i];
            obstacle.Tick(dt);

            if (obstacle.IsPastPlayer)
            {
                pooler.Release(obstacle);
            }
        }
    }
}