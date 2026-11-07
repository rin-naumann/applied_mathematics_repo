using UnityEngine;

public class PerspectiveObject : MonoBehaviour
{
    [SerializeField] private bool randomizeColor = true;
    public Vector3 position;
    public float Scale { get; private set; } = 1f;
    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && randomizeColor)
        {
            spriteRenderer.color = GetRandomColor();
        }
    }

    private void Update()
    {
        float depth = CameraComponent.focalLenth + position.z;

        bool visible = depth > 0.01f;
        if (spriteRenderer != null) spriteRenderer.enabled = visible;
        if (!visible) return;

        Scale = CameraComponent.focalLenth / depth;

        transform.localScale = baseScale * Scale;
        transform.position = new Vector2(position.x, position.y) * Scale;

        if (spriteRenderer != null) spriteRenderer.sortingOrder = -Mathf.RoundToInt(position.z);
    }

    private Color GetRandomColor() => new Color(Random.value, Random.value, Random.value);
}