using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private S_EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponentInParent<S_EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyAI.playerDetected = true;
            enemyAI.player = collision.transform;
            Debug.Log("Player entered vision range!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyAI.playerDetected = false;
            Debug.Log("Player exited vision range.");
        }
    }
}
