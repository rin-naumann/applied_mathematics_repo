using UnityEngine;

public class FinishZoneScript : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] private float finishDistance = 1.5f;
    [SerializeField] private GameObject finishArea;
    [SerializeField] private GameObject winScreen;

    void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        finishArea.transform.localScale = new Vector3(finishDistance * 2, finishDistance * 2, 1);
        winScreen.SetActive(false);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < finishDistance) winScreen.SetActive(true);
    }
}
