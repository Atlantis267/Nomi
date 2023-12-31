using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Sphere : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            BoostSystem playerboostsystem = other.GetComponent<BoostSystem>();

            if (playerboostsystem != null)
            {
                playerboostsystem.AddToBoost();
                gameObject.SetActive(false);
            }
        }
    }
}


