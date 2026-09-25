using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    [SerializeField] private float hitRadius = 0.3f;

    private Transform target;
    private Vector3 previousPosition;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Bullet couldn't find a GameObject tagged 'Player'!");
            return;
        }
        target = playerObj.transform;
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (target == null) return; // guard so it doesn't throw if Start() failed

        previousPosition = transform.position;
        transform.position += transform.right * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        float dist = Vector3.Distance(ClosestPointOnSegment(previousPosition, transform.position, target.position), target.position);

        if (HitPlayer())
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private bool HitPlayer()
    {
        Vector2 closestPoint = ClosestPointOnSegment(previousPosition, transform.position, target.position);
        return Vector2.Distance(closestPoint, target.position) < hitRadius;
    }

    private Vector2 ClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 point)
    {
        Vector2 segment = b - a;
        float segmentLengthSquared = segment.sqrMagnitude;
        if (segmentLengthSquared == 0f) return a;

        float t = Vector2.Dot(point - a, segment) / segmentLengthSquared;
        t = Mathf.Clamp01(t);
        return a + segment * t;
    }
}