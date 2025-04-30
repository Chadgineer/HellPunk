using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public SceneManagement sceneManagement;

    public float jumpSpeed = 7.5f;
    public float moveSpeed = 5f;
    public bool key = false;
    public bool PlatformMoving = false;
    private bool attackOnCooldown;
    public int direction; 


    [SerializeField] private float Health = 100;
    private bool Alive = true;

    [SerializeField] private bool grounded = true;

    public GameObject GateBlocker;
    public GameObject meleeAttack;
    public Animator animator;
    public UnityEngine.UI.Image healthBar;

    public Rigidbody2D rigidbodyPlayer;
    private Vector3 originalScale;

    public float Charge;
    public GameObject projectile;
    private bool projectileCooldown;

    private float damageCooldown = 0.7f; 
    private float lastHitTime = -999f;

    private void Start()
    {
        originalScale = transform.localScale;
        Health = 100;
    }

    private void FixedUpdate()
    {
        rigidbodyPlayer.freezeRotation = true;
        HandleMovement();
        MoveSpeedConditions();
        GateOpener();
        MeleeAttack();
        AliveStatus();
        RangedAttack();
    }

    private void HandleMovement()
    {
        Vector2 newVelocity = rigidbodyPlayer.velocity;


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newVelocity.x = -moveSpeed;
            transform.localScale = originalScale;
            direction = 1;
            animator.SetInteger("AnimState", 2);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newVelocity.x = moveSpeed;
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            direction = 2;
            animator.SetInteger("AnimState", 2);
        }
        else
        {
            newVelocity.x = 0;
            animator.SetInteger("AnimState", 0);
        }
        if (grounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            newVelocity.y = jumpSpeed;
            grounded = false;
        }
        rigidbodyPlayer.velocity = newVelocity;
    }

    void AliveStatus()
    {
        if (Health > 0) { Alive = true; }
        else { Alive = false; }

        if (Health > 100) { Health = 100; }

        if (Alive == false) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

        healthBar.fillAmount = Health;
    }

    public void MoveSpeedConditions()
    {
        if (PlatformMoving == true) { moveSpeed = 5f; }
        else { moveSpeed = 5f; }
    }
    public void GateOpener()
    {
        if (key == true) { GateBlocker.SetActive(false); }
    }
    public void RangedAttack()
    {
        if (Input.GetMouseButton(1))
        {
            StartCoroutine(ProjectileAttack());
        }
    }

    public void MeleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && !attackOnCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        attackOnCooldown = true;

        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.5f);

        meleeAttack.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        meleeAttack.SetActive(false);
        animator.SetBool("Attack", false);

        // Opsiyonel: cooldown süresi (örneðin 0.3 saniye)
        yield return new WaitForSeconds(0.3f);

        attackOnCooldown = false;
    }

    IEnumerator TakingDamage()
    {
        Health -= 12.5f;
        yield return new WaitForSeconds(0.5f);
    }

   

    IEnumerator ProjectileAttack() 
    {

        if (projectileCooldown == false && Charge > 0)
        { 
            Charge--;
            Instantiate(projectile, transform.position, quaternion.identity);
            projectileCooldown = true;
            
            yield return new WaitForSeconds(0.9f);
            projectileCooldown = false;  
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            float angle = Vector2.Angle(contact.normal, Vector2.up);

            if (angle < 45f)
            {
                grounded = true;
            }
        }

        if (collision.collider.CompareTag("Key"))
        {
            key = true;
            Destroy(collision.gameObject);
        }

        if (collision.collider.CompareTag("Lava"))
        {
            Health -= 999;
        }
        if (collision.collider.CompareTag("PlatformMoving"))
        {
            PlatformMoving = true;
        }
        if (collision.collider.CompareTag("Gate"))
        {
            sceneManagement.NextLevel();
        }

        if (collision.collider.CompareTag("EnemyAttackCollider"))
        {
            if (Time.time - lastHitTime > damageCooldown)
            {
                Health -= 20;
                lastHitTime = Time.time;
            }
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            float angle = Vector2.Angle(contact.normal, Vector2.up);

            if (angle < 45f)
            {
                grounded = true;
                return;
            }
        }

        if (collision.collider.CompareTag("Holdable"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;

        if (collision.collider.CompareTag("PlatformMoving"))
        {
            PlatformMoving = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HP Collectible"))
        {
            if (Health < 100)
            {
                Health += 50;
                Destroy(collision.gameObject);
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(TakingDamage());
        }

        if (collision.CompareTag("Charge"))
        {
            Charge++;
            Destroy(collision.gameObject);
        }

    }

}
