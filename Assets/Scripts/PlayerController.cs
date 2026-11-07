using UnityEngine;

[RequireComponent(typeof(PerspectiveObject))]
public class PlayerController : MonoBehaviour
{
    [Header("Lanes")]
    [SerializeField] private float laneSmoothTime = 0.1f; // lower = snappier

    [Header("Jump")]
    [SerializeField] private float groundY = -3f;
    [SerializeField] private float jumpVelocity = 9f;
    [SerializeField] private float gravity = 30f;

    public PerspectiveObject Perspective { get; private set; }
    public int CurrentLane { get; private set; } = 1;   // start in the middle
    public float Height => Perspective.position.y - groundY; // 0 when on the ground
    public bool IsGrounded => Height <= 0.001f;

    private float velocityX;   // SmoothDamp's internal state
    private float velocityY;

    private void Awake()
    {
        Perspective = GetComponent<PerspectiveObject>();
        Perspective.position = new Vector3(Lanes.GetX(CurrentLane), groundY, 0f);
    }

    private void Update()
    {
        ReadInput();
        MoveBetweenLanes();
        ApplyJump();
    }

    private void ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            CurrentLane = Mathf.Max(CurrentLane - 1, 0);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            CurrentLane = Mathf.Min(CurrentLane + 1, Lanes.Count - 1);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && IsGrounded)
            velocityY = jumpVelocity;
    }

    private void MoveBetweenLanes()
    {
        float targetX = Lanes.GetX(CurrentLane);
        Perspective.position.x = Mathf.SmoothDamp(
            Perspective.position.x, targetX, ref velocityX, laneSmoothTime);
    }

    private void ApplyJump()
    {
        // Only integrate physics while airborne or just launched.
        if (IsGrounded && velocityY <= 0f)
        {
            velocityY = 0f;
            Perspective.position.y = groundY;
            return;
        }

        velocityY -= gravity * Time.deltaTime;
        Perspective.position.y += velocityY * Time.deltaTime;

        if (Perspective.position.y <= groundY)
        {
            Perspective.position.y = groundY;
            velocityY = 0f;
        }
    }
}