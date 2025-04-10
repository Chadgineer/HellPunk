using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MovingPlatform : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float moveSpeed = 2f;

    public S_LeverScript leverTrigger;

    private bool moving = false;
    private bool goingToB = false;

    private Vector3 target;

    void Start()
    {
        transform.position = pointA;
        target = pointA;
    }

    void Update()
    {
        if (leverTrigger != null)
        {
            if (leverTrigger.leverActive && !moving && transform.position != pointB)
            {
                target = pointB;
                moving = true;
                goingToB = true;
            }
            else if (!leverTrigger.leverActive && !moving && transform.position == pointB)
            {
                target = pointA;
                moving = true;
                goingToB = false;
            }
        }

        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                moving = false;
            }
        }
    }
}
