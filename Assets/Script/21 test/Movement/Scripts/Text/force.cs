using System;
using System.Collections;
using UnityEngine;

public class force : MonoBehaviour
{
    public Transform center;
    Vector3 Force;
    public float pullForce;
    public float refreshRate;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if(other.GetComponent<Rigidbody>() && other.tag == "Player")
        {
            Force = center.position - other.transform.position;
            Force = Quaternion.Euler(0, refreshRate, 0) * Force;
            other.GetComponent<Rigidbody>().AddForce(Force.normalized * pullForce * Time.deltaTime);
        }
    }



}
