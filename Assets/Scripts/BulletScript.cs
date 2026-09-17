using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;         // Speed of the rocket
    [SerializeField] private float lifetime = 5f;       // Lifetime of the rocket

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Destroy the rocket after its lifetime expires
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy(gameObject);
    }
}
