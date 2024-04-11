using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Deathvalue : MonoBehaviour
    {
        private Player player;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Die();
                }
            }
        }
    }
}
