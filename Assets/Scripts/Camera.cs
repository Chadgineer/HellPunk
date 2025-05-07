using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float minX = -500f;
    public float maxX = 500f;
    public float minY = -500f;
    public float maxY = 500f;

    void LateUpdate()
    {
        if (target != null)
        {
            float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(target.position.y, minY, maxY);


            transform.position = new Vector3(clampedX, clampedY + 1, transform.position.z);
        }
    }
    
}
