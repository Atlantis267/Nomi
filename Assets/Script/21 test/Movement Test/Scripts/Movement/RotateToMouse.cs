using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public Transform target;
    public float turnspeed = 0.1f;
    Quaternion rotGoal;
    Vector3 direction;
    void Update()
    {
        direction = (target.position - transform.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnspeed);
    }
}
