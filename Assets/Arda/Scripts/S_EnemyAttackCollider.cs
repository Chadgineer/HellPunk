using UnityEngine;

public class S_EnemyAttackCollider : MonoBehaviour
{
    private S_EnemyAI enemyAI;

    void Start()
    {
        enemyAI = GetComponentInParent<S_EnemyAI>(); // Assumes this collider is a child of the enemy
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemyAI.canDealDamage)
        {
            // Deal damage to the player (you can access their script here if needed)
            Debug.Log("Player Hit!");

            enemyAI.canDealDamage = false; // Prevent more damage during this attack
        }
    }
}