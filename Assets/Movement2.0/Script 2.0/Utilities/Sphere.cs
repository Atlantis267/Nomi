using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Sphere : MonoBehaviour
    {
        public GameObject canvas;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canvas.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            BoostSystem playerboostsystem = other.GetComponent<BoostSystem>();
            if (playerboostsystem != null)
            {
                if (Input.GetKeyDown("f"))
                {
                    playerboostsystem.AddToBoost();
                }              
            }
        }
        private void OnTriggerExit(Collider other)
        {
            canvas.SetActive(false);
        }
    }
}


