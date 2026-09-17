using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject upgradePrefab;  // Upgrade prefab
    [SerializeField] private float edgeMargin = 0.5f;   // Margin from screen edges, qol so the upgrades 
                                                        // don't spawn partially off-screen
    [SerializeField] private float spawnInterval = 3f; // Interval between spawns
    private float timer;                                // Timer for spawning upgrades

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnUpgrade();
            timer = spawnInterval;
        }
    }


    void SpawnUpgrade()
    {
        float halfW = ScreenBounds.HalfWidth - edgeMargin;
        float halfH = ScreenBounds.HalfHeight - edgeMargin;

        Vector3 spawnPosition = new Vector3(
            Random.Range(-halfW, halfW),
            Random.Range(-halfH, halfH),
            0
        );
        Instantiate(upgradePrefab, spawnPosition, Quaternion.identity);
    }
}
