using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject left, right;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 60f;

    [Header("Health")]
    public int maxHits = 5;
    private int hits = 0;
    public float TimeAlive { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI hitsText;
    [SerializeField] private TextMeshProUGUI timeText;

    void Start() => hitsText.text = $"Hits: {hits} / {maxHits}";

    void Update()
    {
        TimeAlive += Time.deltaTime;
        timeText.text = $"Time: {TimeAlive:F1}s";
        float input = Input.GetAxisRaw("Horizontal");

        transform.position += transform.up * speed * Time.deltaTime;
        Steer(input);
        SteerIndicator(input);
    }

    void Steer(float input)
    {

        Quaternion turn = Quaternion.AngleAxis(-input * turnSpeed * Time.deltaTime, Vector3.forward);
        transform.rotation = turn * transform.rotation;
    }

    void SteerIndicator(float input)
    {
        left.SetActive(input < 0);
        right.SetActive(input > 0);
    }

    public void TakeHit()
    {
        hits++;
        hitsText.text = $"Hits: {hits} / {maxHits}";
        if (hits >= maxHits)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
