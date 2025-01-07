using JetBrains.Annotations;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public SceneManagement sceneManagement;

    public float jumpSpeed = 10f;
    public float moveSpeed = 7.5f;
    public bool key = false;
    public bool PlatformMoving = false;
    public int direction; // 1 = left, 2 = right

    [SerializeField] private float Health = 100;
    private bool Alive = true;

    [SerializeField] private bool grounded = true;
    [SerializeField] private bool touchingWall = false;

    public GameObject GateBlocker;
    public GameObject LeftMelee;
    public GameObject RightMelee;

    public Rigidbody2D rigidbodyPlayer;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        rigidbodyPlayer.freezeRotation = true;
        HandleMovement();
        MoveSpeedConditions();
        GateOpener();
        MeleeAttack();
    }

    private void HandleMovement()
    {
        Vector2 newVelocity = rigidbodyPlayer.velocity;

        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newVelocity.x = -moveSpeed;
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            direction = 1;
                
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
                newVelocity.x = moveSpeed;
                transform.localScale = originalScale;
            direction = 2;
        }
        else
        {
                newVelocity.x = 0;
        }
        if (grounded && !touchingWall && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
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

        if (Alive == false) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    }

    public void MoveSpeedConditions() 
    {
        if (PlatformMoving == true) { moveSpeed = 20f; }
        else { moveSpeed = 7.5f; }
    }
    public void GateOpener() 
    { 
        if (key == true) { GateBlocker.SetActive(false); }
    }

    public void MeleeAttack() 
    {
        if (Input.GetMouseButton(0))    
        {
            if (direction == 1) { StartCoroutine(LeftAttack()); }
            else if (direction == 2) { StartCoroutine(RightAttack()); }
        }
    }
    IEnumerator TakingDamage() 
    { 
        Health -=25;
        yield return new WaitForSeconds(0.5f);
        
    }
    IEnumerator RightAttack() 
    { 
        RightMelee.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        RightMelee.SetActive(false);
        yield return new WaitForSeconds(1); 
    }

    IEnumerator LeftAttack()
    {
        LeftMelee.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        LeftMelee.SetActive(false);
        yield return new WaitForSeconds(1);
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
            else if (angle > 45f && angle < 135f)
            {
                touchingWall = true;
            }
        }

        if (collision.collider.CompareTag("Enemy")) 
        { 
        
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
        touchingWall = false;
        
        if (collision.collider.CompareTag("PlatformMoving"))
        {
            PlatformMoving = false;
        }
    }
}