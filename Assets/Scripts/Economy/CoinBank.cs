using TMPro;
using UnityEngine;

public class CoinBank : MonoBehaviour
{
    public static CoinBank Instance { get; private set; }

    [SerializeField] private RectTransform coinElement;
    [SerializeField] private TMP_Text balanceLabel;
    [SerializeField] private int startingBalance = 0;
    [SerializeField] private float countDuration = 0.5f;
    [SerializeField] private float punchDuration = 0.35f;
    [SerializeField] private float punchAmount = 0.35f;

    private int balance;
    private float displayed;
    private float countStart;
    private float countT = 1f;
    private float punchT = 1f;
    private Vector3 baseScale;
    private Canvas canvas;
    private Camera cam;

    public int Balance => balance;

    private void Awake()
    {
        Instance = this;
        canvas = coinElement.GetComponentInParent<Canvas>();
        cam = Camera.main;
        baseScale = coinElement.localScale;

        balance = startingBalance;
        displayed = balance;
        Refresh();
    }

    public Vector3 TargetWorldPosition
    {
        get
        {
            Vector3 p = coinElement.position;
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                p = cam.ScreenToWorldPoint(new Vector3(p.x, p.y, Mathf.Abs(cam.transform.position.z)));
            p.z = 0f;
            return p;
        }
    }

    public void AddCoins(int amount)
    {
        balance += amount;
        countStart = displayed;
        countT = 0f;
        punchT = 0f;
    }

    private void Update()
    {
        if (countT < 1f)
        {
            countT += Time.deltaTime / countDuration;
            displayed = Mathf.Lerp(countStart, balance, Easing.OutCubic(Mathf.Clamp01(countT)));
            Refresh();
        }

        if (punchT < 1f)
        {
            punchT += Time.deltaTime / punchDuration;
            float p = Mathf.Sin(Mathf.PI * Easing.OutCubic(Mathf.Clamp01(punchT)));
            coinElement.localScale = baseScale * (1f + punchAmount * p);
        }
        else
        {
            coinElement.localScale = baseScale;
        }
    }

    private void Refresh() => balanceLabel.text = Mathf.RoundToInt(displayed).ToString();
}