using UnityEngine;

public class WinAreaScript : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    public Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
