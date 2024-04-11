using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class CheckPoint : MonoBehaviour
    {
        private GameMaster gm;
        private void Start()
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gm.lastCheckPointPos = transform.position;
                gm.lastCheckPointRotate = transform.forward;
            }
        }
    }
}
