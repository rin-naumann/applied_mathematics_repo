using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private int maxHp = 20;

    public int Current { get; private set; }
    public int Max => maxHp;

    public event Action<int, int> Changed;   // (current, max)
    public event Action Died;

    private void Awake()
    {
        Instance = this;
        Current = maxHp;
    }

    public void TakeDamage(int amount)
    {
        if (Current <= 0) return;

        Current = Mathf.Max(0, Current - amount);
        Changed?.Invoke(Current, maxHp);

        if (Current == 0)
            Died?.Invoke();
    }
}