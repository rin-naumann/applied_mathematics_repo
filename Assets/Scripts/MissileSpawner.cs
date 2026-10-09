using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public MissilePool missilePool;
    public PlayerController player;
    public Camera cam;

    [Header("Spawning")]
    public float spawnInterval = 3f;
    public int baseMissiles = 1;
    public float difficultyStep = 10f;

    private float timer;

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        // Scaling difficulty: +1 missile for every 10 seconds survived
        int count = baseMissiles + Mathf.FloorToInt(player.TimeAlive / difficultyStep);

        for (int i = 0; i < count; i++)
        {
            missilePool.Get(GetPositionOutsideCamera(), player);
        }
    }

    // Picks a viewport point outside the 0..1 range so it's off-screen
    Vector3 GetPositionOutsideCamera()
    {
        float vx, vy;

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:  vx = Random.Range(-0.3f, -0.1f); vy = Random.Range(-0.3f, 1.3f); break; // left
            case 1:  vx = Random.Range(1.1f, 1.3f);   vy = Random.Range(-0.3f, 1.3f); break; // right
            case 2:  vx = Random.Range(-0.3f, 1.3f);  vy = Random.Range(1.1f, 1.3f);  break; // top
            default: vx = Random.Range(-0.3f, 1.3f);  vy = Random.Range(-0.3f, -0.1f); break; // bottom
        }

        Vector3 pos = cam.ViewportToWorldPoint(new Vector3(vx, vy, 0f));
        pos.z = 0f; // keep on the 2D plane
        return pos;
    }
}