using UnityEngine;

public class PlayerController : MonoBehaviour, ICollidable
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.8f;

    private float velocityY = 0f;
    private bool isGrounded = false;

    public AABBCollider Collider { get; private set; }

    void Awake() => Collider = GetComponent<AABBCollider>();
    void OnEnable() => CollisionManager.Register(this);
    void OnDisable() => CollisionManager.Unregister(this);

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocityY += gravity * Time.deltaTime;
        transform.position += new Vector3(0f, velocityY * Time.deltaTime, 0f);

        isGrounded = false;
    }

    public void OnColliderEnter(ICollidable other)
    {
        if (other is not SolidObject) return;

        AABBCollider player = Collider;
        AABBCollider otherCollider = other.Collider;

        float overlapX = Mathf.Min(player.max.x, otherCollider.max.x) - Mathf.Max(player.min.x, otherCollider.min.x);
        float overlapY = Mathf.Min(player.max.y, otherCollider.max.y) - Mathf.Max(player.min.y, otherCollider.min.y);

        float myCenterX = (player.min.x + player.max.x) * 0.5f;
        float myCenterY = (player.min.y + player.max.y) * 0.5f;
        float theirCenterX = (otherCollider.min.x + otherCollider.max.x) * 0.5f;
        float theirCenterY = (otherCollider.min.y + otherCollider.max.y) * 0.5f;

        if (overlapY < overlapX)
        {
            if (myCenterY > theirCenterY)
            {
                transform.position += new Vector3(0f, overlapY, 0f);
                velocityY = 0f;
                isGrounded = true;
            }
            else
            {
                transform.position -= new Vector3(0f, overlapY, 0f);
                velocityY = Mathf.Min(velocityY, 0f);
            }
        }
        else
        {
            float direction = myCenterX > theirCenterX ? 1f : -1f;
            transform.position += new Vector3(overlapX * direction, 0f, 0f);
        }

        player.Recalculate();
    }
}