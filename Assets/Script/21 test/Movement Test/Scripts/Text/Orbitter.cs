using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitter : MonoBehaviour
{
    //public GameObject[] spheres;
    readonly float G = 100f;
    [SerializeField]
    //bool IsElipticalOrbit = false;

    //void Update()
    //{
    //    foreach (GameObject s in spheres)
    //    {
    //        Vector3 difference = this.transform.position - s.transform.position;
    //        float dist = difference.magnitude;
    //        Vector3 gravityDirection = difference.normalized;
    //        float gravity = 6.7f * (this.transform.localScale.x * s.transform.localScale.x * 80f) / (dist * dist);
    //        Vector3 gravityVector = (gravityDirection * gravity);
    //        s.transform.GetComponent<Rigidbody>().AddForce(s.transform.forward, ForceMode.Acceleration);
    //        s.transform.GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Vector3 difference = this.transform.position - other.transform.position;
        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = G * (this.transform.localScale.x * other.transform.localScale.x * 80f) / (dist * dist);
        Vector3 gravityVector = (gravityDirection * gravity);

        other.transform.GetComponent<Rigidbody>().AddForce(other.transform.forward, ForceMode.Acceleration);
        other.transform.GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
    }
}
