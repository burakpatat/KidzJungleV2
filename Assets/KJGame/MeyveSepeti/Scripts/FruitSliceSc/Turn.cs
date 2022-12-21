using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
 
    float turnSpeed = 5f;
    void Update()
    {
        transform.Rotate(0f, 0f, 0.1f * turnSpeed, Space.World);
    }
}
