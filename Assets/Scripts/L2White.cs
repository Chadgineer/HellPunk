using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2White : MonoBehaviour
{
    private bool whiteTouches;
    public GameObject white;

    private void Update()
    {
        if (whiteTouches)
        { white.SetActive(true); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("white"))
        {
            whiteTouches = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("white"))
        {
            whiteTouches = false;
        }
    }
}
