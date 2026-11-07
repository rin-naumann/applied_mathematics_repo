using System;
using UnityEngine;

[RequireComponent(typeof(AABBCollider))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damagePerHit = 5f;
    [SerializeField] private float regenPerSecond = 1f;

    public float Current { get; private set; }
    public float Max => maxHealth;

    public event Action<float, float> HealthChanged; // (current, max)

    private AABBCollider hitbox;

    private void Awake()
    {
        hitbox = GetComponent<AABBCollider>();
        Current = maxHealth;
    }

    private void OnEnable()  { hitbox.Collided += OnCollided; }
    private void OnDisable() { hitbox.Collided -= OnCollided; }

    private void OnCollided(AABBCollider other)
    {
        if (!other.TryGetComponent<Obstacle>(out _)) return;
        SetHealth(Current - damagePerHit);
    }

    private void Update()
    {
        if (Current >= maxHealth) return; // nothing to regenerate, no UI updates either
        SetHealth(Current + regenPerSecond * Time.deltaTime);
    }

    private void SetHealth(float value)
    {
        Current = Mathf.Clamp(value, 0f, maxHealth);
        HealthChanged?.Invoke(Current, maxHealth);
    }
}