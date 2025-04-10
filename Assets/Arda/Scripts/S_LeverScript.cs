using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LeverScript : MonoBehaviour
{
    public bool leverActive = false;
    private bool hasBeenActivated = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            leverActive = !leverActive; // toggles on/off each time
            hasBeenActivated = true;

        }
    }
}
