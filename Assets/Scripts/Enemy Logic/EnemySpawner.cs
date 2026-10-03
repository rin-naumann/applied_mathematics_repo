using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool pool;
    [SerializeField] private BezierPath path;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float enemySpeed = 2.5f;

    private float timer;

    private void Start() => timer = startDelay;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            pool.Spawn(path, enemySpeed);
            timer += spawnInterval;
        }
    }
}