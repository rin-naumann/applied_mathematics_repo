using UnityEngine;
using UnityEngine.Pool;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private float flightTime = 0.8f;
    [SerializeField] private float arcHeight = 2f;

    private IObjectPool<Coin> pool;
    private Vector3 start;
    private Vector3 control;
    private float t;

    public void SetPool(IObjectPool<Coin> ownerPool) => pool = ownerPool;

    public void Init(Vector3 position)
    {
        transform.position = position;
        start = position;
        t = 0f;

        Vector3 end = CoinBank.Instance.TargetWorldPosition;
        Vector3 toEnd = end - start;
        Vector3 dir = toEnd.sqrMagnitude > 0.0001f ? toEnd.normalized : Vector3.up;
        Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0f);
        float side = Random.value < 0.5f ? -1f : 1f;

        control = (start + end) * 0.5f + perpendicular * (arcHeight * side * Random.Range(0.5f, 1f));
    }

    private void Update()
    {
        t += Time.deltaTime / flightTime;
        float k = Easing.InQuad(Mathf.Clamp01(t));

        Vector3 end = CoinBank.Instance.TargetWorldPosition;
        transform.position = BezierMath.Quadratic(start, control, end, k);

        if (t >= 1f)
        {
            CoinBank.Instance.AddCoins(value);
            pool.Release(this);
        }
    }
}