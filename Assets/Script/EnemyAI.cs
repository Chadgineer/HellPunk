using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 0.5f;
    private bool movingToB = true;
    private float t = 0f;
    public float followSpeed = 1;
    public Transform PlayerWhere;
    private Vector3 originalScale;

    [SerializeField] private float Health = 50;
    private bool Alive = true;

    [SerializeField] private Rigidbody2D rb;

    public bool playerDetected;

    private Vector3 previousPosition;
    [HideInInspector] public Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (playerDetected == true) { speed = followSpeed; }
        else { speed = 0.5f; }

        pingpong();
        EnemyDirection();
    }

    void pingpong()
    {
        t += (movingToB ? 1 : -1) * speed * Time.deltaTime;
        t = Mathf.Clamp01(t);
        transform.position = Vector3.Lerp(pointA, pointB, t);

        if (t >= 1f) movingToB = false;
        else if (t <= 0f) movingToB = true;
    }
    void EnemyDirection()
    {
        if (PlayerWhere.position.x == pointA.x) { transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); }
        else if (PlayerWhere.position.x == pointB.x) { transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); }
    }

    void AliveStatus() 
    { 
        if (Health > 0) { Alive = true; }
        else {  Alive = false; }

        if (Alive == false) { gameObject.SetActive(false); } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
        }

        if (collision.gameObject.CompareTag("Attack"))
        { Health -= 100; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}