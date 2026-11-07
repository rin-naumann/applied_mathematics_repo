using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float spawnZ = 2000f;
    [SerializeField] private Vector2 spawnInterval = new Vector2(0.9f, 1.6f);

    [Header("Relative chance of a wave having 1 / 2 / 3 obstacles")]
    [SerializeField] private Vector3 waveWeights = new Vector3(5f, 3.5f, 1.5f);

    private readonly int[] laneBuffer = new int[Lanes.Count];
    private float timer;

    private void Start()
    {
        timer = Random.Range(spawnInterval.x, spawnInterval.y);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer += Random.Range(spawnInterval.x, spawnInterval.y);
        SpawnWave();
    }

    private void SpawnWave()
    {
        int count = RollWaveSize();
        ShuffleLanes();

        for (int i = 0; i < count; i++)
        {
            Spawn(laneBuffer[i]);
        }
    }

    private int RollWaveSize()
    {
        float roll = Random.value * (waveWeights.x + waveWeights.y + waveWeights.z);
        if (roll < waveWeights.x) return 1;
        if (roll < waveWeights.x + waveWeights.y) return 2;
        return 3;
    }

    private void ShuffleLanes()
    {
        for (int i = 0; i < laneBuffer.Length; i++) laneBuffer[i] = i;

        for (int i = laneBuffer.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (laneBuffer[i], laneBuffer[j]) = (laneBuffer[j], laneBuffer[i]);
        }
    }

    private void Spawn(int lane)
    {
        var position = new Vector3(Lanes.GetX(lane), Lanes.groundY, spawnZ);
        ObstacleManager.Instance.Spawn(position);
    }
}