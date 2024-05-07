using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class RestoreEnergy : MonoBehaviour
    {
        public GameObject canvas;
        public bool isplayer;
        private bool interactInput;
        PlayerInputActions Inputon;
        [SerializeField] private Vector3 boxscale;        

        private void Awake()
        {
            Inputon = new PlayerInputActions();
            Inputon.Player.Interact.started += OnInteractStarted;
            Inputon.Player.Interact.canceled += OnInteractCanceled;
        }

        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            interactInput = false;
        }
        private void OnInteractStarted(InputAction.CallbackContext context)
        {
            interactInput = true;
        }
        private void OnEnable()
        {
            Inputon.Player.Enable();
        }
        private void OnDisable()
        {
            Inputon.Player.Disable();
        }

        private void Update()
        {
            Interact();
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, boxscale);
        }

        private void Interact()
        {
            Collider[] Zones = Physics.OverlapBox(transform.position, boxscale);
            foreach(Collider player in Zones)
            {
                if (player.tag =="Player")
                {
                    isplayer = true;
                    BoostSystem playerboostsystem = player.GetComponent<BoostSystem>();
                    if (interactInput)
                    {
                        playerboostsystem.AddToBoost();
                    }
                    break;
                }
                else
                {
                    isplayer = false;
                }
            }
            if (isplayer)
            {
                canvas.SetActive(true);
            }
            else
            {
                canvas.SetActive(false);
            }
        }        
    }
}


