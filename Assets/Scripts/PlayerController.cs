using UnityEngine;

/// <summary>
/// Messy ahh code but it works
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;      // Player movement speed
    [SerializeField] private GameObject bulletPrefab;   // Bullet prefab
    [SerializeField] private int bulletCount = 2;       // Bullet count
    [SerializeField] private float fireRate = 1f;       // Bullet fire rate
    private float bulletSpacing;                        // Bullet spacing
    private const int MAX_BULLET_COUNT = 8;             // Maximum bullet count
    private float timer;                                // Timer for firing bullets

    public static PlayerController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this; 
        else Destroy(gameObject);
    }

    void Start() 
    {
        timer = fireRate;
        bulletSpacing = 360 / bulletCount;
    }

    void Update()
    {
        PlayerMovement();
        PlayerFiring();
    }

    // Movement
    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector2 (horizontalInput, verticalInput).normalized * Time.deltaTime;
        transform.Translate(movement * moveSpeed);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -ScreenBounds.HalfWidth, ScreenBounds.HalfWidth);
        pos.y = Mathf.Clamp(pos.y, -ScreenBounds.HalfHeight, ScreenBounds.HalfHeight);
        transform.position = pos;
    }

    // Firing
    void PlayerFiring()
    {
        bulletSpacing = 360 / bulletCount;
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * bulletSpacing;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Instantiate(bulletPrefab, transform.position, rotation);
            }
            timer = fireRate;
        }
    }

    // Level Up
    // I have one hour and I can't be bothered to use action events ToT
    public void LevelUp()
    {
        if (bulletCount < MAX_BULLET_COUNT) bulletCount++;
    }
}
