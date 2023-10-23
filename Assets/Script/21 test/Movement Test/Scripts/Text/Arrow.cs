using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Mathf.Deg2Rad * speed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Mathf.Deg2Rad * speed;
       
        transform.RotateAround(Vector3.up, -mouseX);
    }
}
