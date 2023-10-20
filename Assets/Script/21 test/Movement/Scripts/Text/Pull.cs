using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour
{
    float xroat, yroat = 0f;
    public Rigidbody ball;
    public float rotatespeed = 5f;
    public LineRenderer line;
    public float shootpower = 30f;
    public float maxpower = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.position = ball.position;
        Vector3 inputPos = new Vector3(xroat, yroat, xroat);
        if (Input.GetMouseButton(0))
        {

            xroat += Input.GetAxis("Mouse X") * rotatespeed;
            //yroat += Input.GetAxis("Mouse Y") * rotatespeed;         
            transform.rotation = Quaternion.Euler(yroat, xroat, 0f);
            line.gameObject.SetActive(true);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + transform.forward * 4f);
            if (yroat < -35f)
            {
                yroat = -35f;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            ball.velocity = transform.forward * shootpower;
            line.gameObject.SetActive(false);
        }

    }
}
