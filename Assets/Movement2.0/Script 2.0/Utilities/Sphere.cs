using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Sphere : MonoBehaviour
    {
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
    }
}


