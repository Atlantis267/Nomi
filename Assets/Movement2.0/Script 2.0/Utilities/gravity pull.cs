using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class gravitypull : MonoBehaviour
    {
        private float G = 100f;
        public Player player;
        private void OnTriggerStay(Collider other)
        {
            player = other.GetComponent<Player>();
            player.CharacterController.enabled = false;

            Vector3 difference = this.transform.position - other.transform.position;
            float dist = difference.magnitude;
            Vector3 gravityDirection = difference.normalized;
            float gravity = G * (this.transform.localScale.x * other.transform.localScale.x * 80f) / (dist * dist);
            Vector3 gravityVector = (gravityDirection * gravity);
            other.transform.GetComponent<Rigidbody>().AddForce(other.transform.forward, ForceMode.Acceleration);
            other.transform.GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
        }
        private void OnTriggerExit(Collider other)
        {
            player = other.GetComponent<Player>();
            player.CharacterController.enabled = true;
        }
    }
}


