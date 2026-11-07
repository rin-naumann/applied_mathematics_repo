using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text hpText;

    private void OnEnable()  { playerHealth.HealthChanged += Refresh; }
    private void OnDisable() { playerHealth.HealthChanged -= Refresh; }

    private void Start()
    {
        Refresh(playerHealth.Current, playerHealth.Max);
    }

    private void Refresh(float current, float max)
    {
        hpFill.fillAmount = current / max;
        hpText.text = $"HP {Mathf.FloorToInt(current)} / {max}";
    }
}