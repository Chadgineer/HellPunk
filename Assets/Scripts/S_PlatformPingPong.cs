using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlatformPingPong : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;

    private bool movingToB = true;
    private float t = 0f;
    public float pingpongSpeed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        PingPong();
    }

    void PingPong()
    {
        t += (movingToB ? 1 : -1) * pingpongSpeed * Time.deltaTime;
        t = Mathf.Clamp01(t);
        transform.position = Vector3.Lerp(pointA, pointB, t);

        if (t >= 1f) movingToB = false;
        else if (t <= 0f) movingToB = true;
    }
}
