using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image realFill;
    [SerializeField] private Image ghostFill;
    [SerializeField] private TMP_Text label;          // optional, e.g. "14 / 20"
    [SerializeField] private float ghostDelay = 0.5f;
    [SerializeField] private float ghostDuration = 0.6f;

    private float ghostStart;
    private float ghostTarget;
    private float holdTimer;
    private float easeT = 1f;   // 1 = not animating

    private void Start()
    {
        PlayerHealth hp = PlayerHealth.Instance;
        hp.Changed += OnHealthChanged;

        float full = (float)hp.Current / hp.Max;
        realFill.fillAmount = ghostFill.fillAmount = full;
        UpdateLabel(hp.Current, hp.Max);
    }

    private void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.Changed -= OnHealthChanged;
    }

    private void OnHealthChanged(int current, int max)
    {
        float target = (float)current / max;

        realFill.fillAmount = target;
        UpdateLabel(current, max);

        if (target >= ghostFill.fillAmount)
        {
            ghostFill.fillAmount = target;
            easeT = 1f;
        }
        else
        {
            ghostStart = ghostFill.fillAmount;
            ghostTarget = target;
            holdTimer = ghostDelay;
            easeT = 0f;
        }
    }

    private void Update()
    {
        if (easeT >= 1f) return;

        if (holdTimer > 0f)
        {
            holdTimer -= Time.deltaTime;
            return;
        }

        easeT += Time.deltaTime / ghostDuration;
        ghostFill.fillAmount = Mathf.Lerp(ghostStart, ghostTarget, Easing.OutCubic(Mathf.Clamp01(easeT)));
    }

    private void UpdateLabel(int current, int max)
    {
        if (label != null) label.text = $"{current} / {max}";
    }
}