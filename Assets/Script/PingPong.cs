using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PingPong : MonoBehaviour
{
    public Vector3 pointA; 
    public Vector3 pointB;
    public float speed = 1f;
    public UnityEvent PlayerContact;

    Player player;

    [HideInInspector] public Vector3 velocity; 

    private Vector3 previousPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        previousPosition = transform.position;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, t);
        
        velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); 
        }
    }
}
