using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyAI : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float patrolSpeed = 0.5f;
    public float followSpeed = 1f;
    public float attackRange = 1f;
    public float attackCooldown = 0.7f;

    private bool movingToB = true;
    private float t = 0f;

    public Transform EnemyLocation;
    private Vector3 originalScale;

    [SerializeField] private float Health = 50;
    private bool Alive = true;
    private bool Death = false;

    [SerializeField] private Rigidbody2D rb;
    public bool playerDetected;

    public Transform player;
    private Animator animator;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    public bool canDealDamage = false;
    public bool isChasing = false; // Flag to track chasing state


    // Attack collider reference
    [SerializeField] private Collider2D attackCollider;

    // Enemy vision range collider reference
    [SerializeField] private Collider2D enemyVisionRange;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>(); // Add Animator component

        // Ensure the attack collider is initially disabled
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    void Update()
    {
        if (!Alive) return;

        // Prevent any movement during the attack cooldown
        if (isAttacking) return; // If the enemy is attacking, skip movement logic

        if (playerDetected && player != null && !isChasing)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                rb.velocity = Vector2.zero;

                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
            }
            else if (!isAttacking)
            {
                ChasePlayer();
            }

            animator.SetInteger("AnimState", 2); // Running (Chasing)
        }
        else if (!isAttacking)
        {
            Patrol();
            // Set the animator state to 'Run' when patrolling
            animator.SetInteger("AnimState", 2);
        }

        EnemyDirection();
        AliveStatus();


    }


    void Patrol()
    {
        t += (movingToB ? 1 : -1) * patrolSpeed * Time.deltaTime;
        t = Mathf.Clamp01(t);
        transform.position = Vector3.Lerp(pointA, pointB, t);

        if (t >= 1f) movingToB = false;
        else if (t <= 0f) movingToB = true;
    }

    void ChasePlayer()
    {
        isChasing = true; // Start chasing the player

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * followSpeed;
        
    }

    IEnumerator Attack()
    {
        isChasing = false;
        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop moving
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze all movement while attacking
        animator.SetTrigger("Attack");

        // Enable the attack collider during the attack
        if (attackCollider != null)
        {
            canDealDamage = true;
            attackCollider.enabled = true;
        }

        // Wait for attack animation to finish or duration of attack
        yield return new WaitForSeconds(attackCooldown);

        // Disable the attack collider after the attack is over
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            canDealDamage = false;
        }

        isAttacking = false;

        

    }

    void EnemyDirection()
    {
        float directionX = rb.velocity.x;

        // If not using Rigidbody2D velocity during patrol, fall back to comparing transform positions
        if (Mathf.Abs(directionX) < 0.01f)
        {
            if (movingToB)
                directionX = -1f;
            else
                directionX = 1f;
        }
       // Only flip if the velocity is significant
        if (Mathf.Abs(directionX) > 0.05f)
        {
            if (directionX > 0)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Flip the scale based on movement direction
        if (directionX > 0)
            transform.localScale = new Vector2(Mathf.Abs(originalScale.x), originalScale.y);
        else if (directionX < 0)
            transform.localScale = new Vector2(-Mathf.Abs(originalScale.x), originalScale.y);

    }

    void AliveStatus()
    {
        Alive = Health > 0;
        if (!Alive)
        {
            Alive = false;
            animator.SetTrigger("Death");
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; // So it doesn't fall or get pushed

            // Set all colliders (including children) to trigger
            Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D col in allColliders)
            {
                col.enabled = false;
            }

            Destroy(gameObject, 5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
            player = collision.transform;
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero; // Stop moving when the player leaves the collision
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            Health -= 100;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
            rb.velocity = Vector2.zero;
        }
    }
}
