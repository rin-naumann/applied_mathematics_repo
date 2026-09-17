using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    // For qol purposes, so the player doesn't have to be directly on top of the upgrade to pick it up
    [SerializeField] private float detectionRadius = 1f;
    private PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        playerController = PlayerController.Instance;

        if (Vector2.Distance(transform.position, playerController.transform.position) < detectionRadius)
        {
            playerController.LevelUp();
            Destroy(gameObject);
        }
    }
}
