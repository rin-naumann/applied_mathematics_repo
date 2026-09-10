using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NoGoZoneScript : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] private float warningDistance = 4f;
    [SerializeField] private float noGoDistance = 2f;
    [SerializeField] private float timeToLose = 5f;
    [SerializeField] GameObject warningArea, LossArea;
    private Coroutine shakeRoutine;
    private float distanceToPlayer;
    private Vector3 originalPosition;

    void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        warningArea.transform.localScale = new Vector3(warningDistance * 2, warningDistance * 2, 1);
        LossArea.transform.localScale = new Vector3(noGoDistance * 2, noGoDistance * 2, 1);
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < noGoDistance)
        {
            ResetScene();
        }
        else if (distanceToPlayer < warningDistance)
        {
            if (shakeRoutine == null) shakeRoutine = StartCoroutine(ShakeZone());
        }
        else if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            shakeRoutine = null;
            warningArea.transform.localPosition = Vector3.zero;
        }
    }

    IEnumerator ShakeZone()
    {
        Vector3 originalLocalPos = warningArea.transform.localPosition;
        float timer = timeToLose;

        while (timer > 0f)
        {
            warningArea.transform.localPosition = originalLocalPos + (Vector3)(Random.insideUnitCircle * 0.1f);
            timer -= Time.deltaTime;
            Debug.Log("Timer: " + timer);
            yield return null;
        }

        ResetScene();
    }

    void ResetScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
